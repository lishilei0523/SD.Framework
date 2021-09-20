import {APP_INITIALIZER, NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {ApplicationTypeDescriptor} from "../values/enums/application-type.descriptor";
import {AppConfigService} from "../services/app-config.service";
import {InitializeAppConfig} from "../services/functions.service";
import {RouteReuseStrategy} from "@angular/router";
import {TabRouteReuseStrategy} from "../services/tab-route-reuse.strategy";
import {AuthenticationService} from "../services/authentication.service";
import {AuthorityDirective} from "../services/authority.directive";

/*SD.Infrastructure模块*/
@NgModule({
    declarations: [ApplicationTypeDescriptor, AuthorityDirective],
    exports: [ApplicationTypeDescriptor, AuthorityDirective],
    providers: [
        AppConfigService,
        AuthenticationService,
        {
            provide: APP_INITIALIZER,
            useFactory: InitializeAppConfig,
            deps: [AppConfigService],
            multi: true
        },
        {
            provide: RouteReuseStrategy,
            useClass: TabRouteReuseStrategy
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthenticationService,
            multi: true
        }
    ]
})
export class InfrastructureModule {

}
