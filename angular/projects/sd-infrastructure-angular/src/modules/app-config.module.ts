import {NgModule, Injectable, APP_INITIALIZER} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RouteReuseStrategy} from "@angular/router";
import {AppConfig, Constants} from "sd-infrastructure-values";
import {TabRouteReuseStrategy} from "../strategies/route-reuse.strategy";


/*应用程序配置服务*/
@Injectable({
    providedIn: 'root'
})
export class AppConfigService {

    /*Http客户端*/
    private httpClient: HttpClient;

    /**
     * 创建应用程序配置服务构造器
     * */
    public constructor(httpClient: HttpClient) {
        this.httpClient = httpClient;
    }

    /**
     * 加载应用程序配置
     * */
    public async loadAppConfig(): Promise<void> {
        let configUrl: string = "assets/app.config.json";
        Constants.appConfig = await this.httpClient.get<AppConfig>(configUrl).toPromise();
    }
}


// @dynamic
/**
 * 初始化应用程序配置函数
 * @param appConfigService - 应用程序配置服务
 * */
export function InitializeAppConfig(appConfigService: AppConfigService) {
    const result = () => appConfigService.loadAppConfig();
    return result;
}


/*应用程序配置模块*/
@NgModule({
    providers: [
        AppConfigService,
        {
            provide: RouteReuseStrategy,
            useClass: TabRouteReuseStrategy
        },
        {
            provide: APP_INITIALIZER,
            useFactory: InitializeAppConfig,
            deps: [AppConfigService],
            multi: true
        }
    ]
})
export class AppConfigModule {

}

