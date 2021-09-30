import {Directive, Input, TemplateRef, ViewContainerRef} from "@angular/core";
import {Constants} from "../values/constants/constants";
import {Membership} from "../values/constants/membership";

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
        if (Membership.loginAuthorityPaths != null && Membership.loginAuthorityPaths.includes(authorityPath)) {
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
