import {NgModule, Injectable, Directive, TemplateRef, ViewContainerRef, Input} from '@angular/core';
import {CanActivate, Router} from "@angular/router";
import {HTTP_INTERCEPTORS, HttpEvent, HttpInterceptor, HttpHandler, HttpRequest,} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Constants, Membership} from "sd-infrastructure-values";


/*身份认证授权指令*/
@Directive({
    selector: '[authorityPath]'
})
export class AuthorityDirective {

    /*模板引用*/
    private readonly template: TemplateRef<any>;

    /*视图容器引用*/
    private readonly viewContainer: ViewContainerRef;

    /*是否已授权*/
    private authorized: boolean;

    /**
     * 依赖注入构造器
     * */
    public constructor(template: TemplateRef<any>, viewContainer: ViewContainerRef) {
        this.template = template;
        this.viewContainer = viewContainer;
        this.authorized = false;
    }

    /**
     * 设置权限路径
     * @param authorityPath - 权限路径
     * */
    @Input()
    public set authorityPath(authorityPath: string) {
        if (Membership.loginAuthorityPaths.includes(authorityPath)) {
            this.authorized = true;
        }
        if (!Constants.appConfig.authorizationEnabled) {
            this.authorized = true;
        }

        if (this.authorized) {
            this.viewContainer.createEmbeddedView(this.template);
        } else if (!this.authorized) {
            this.viewContainer.clear();
        }
    }
}


/*身份认证服务*/
@Injectable({
    providedIn: 'root'
})
export class AuthenticationService implements HttpInterceptor {

    /**
     * 拦截请求
     * */
    public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let publicKey: string = Membership.loginInfo == null ? "" : Membership.loginInfo.publicKey;
        if (request.method == "GET" && publicKey) {
            let headers = {
                "CurrentPublicKey": publicKey
            };
            return next.handle(request.clone({setHeaders: headers}));
        }
        if (request.method == "POST") {
            let contentType: string = "application/json";
            if (publicKey) {
                let headers = {
                    "Content-Type": contentType,
                    "CurrentPublicKey": publicKey
                };
                return next.handle(request.clone({setHeaders: headers}));
            } else {
                let headers = {
                    "Content-Type": contentType
                };
                return next.handle(request.clone({setHeaders: headers}));
            }
        }

        return next.handle(request);
    }
}


/*身份认证守卫器*/
@Injectable({
    providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate {

    /*路由器*/
    private readonly router: Router;

    /**
     * 依赖注入构造器
     * */
    public constructor(router: Router) {
        this.router = router;
    }

    /**
     * 是否可激活
     * */
    public async canActivate(): Promise<boolean> {
        if (Membership.loginInfo) {
            return true;
        } else {
            await this.router.navigate(["/Login"])
            return false;
        }
    }
}


/*应用程序身份认证模块*/
@NgModule({
    declarations: [AuthorityDirective],
    exports: [AuthorityDirective],
    providers: [
        AuthenticationService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthenticationService,
            multi: true
        }
    ]
})
export class AppAuthenticationModule {

}
