import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {Membership} from "../values/constants/membership";

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
                "PublicKey": publicKey
            };
            return next.handle(request.clone({setHeaders: headers}));
        }
        if (request.method == "POST") {
            let headers: any = {};
            if (Object.prototype.toString.call(request.body) != "[object FormData]") {
                headers["Content-Type"] = "application/json";
            }
            if (publicKey) {
                headers["PublicKey"] = publicKey;
                return next.handle(request.clone({setHeaders: headers}));
            } else {
                return next.handle(request.clone({setHeaders: headers}));
            }
        }

        return next.handle(request);
    }
}
