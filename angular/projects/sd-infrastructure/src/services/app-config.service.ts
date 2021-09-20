import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Constants} from "../values/constants/constants";
import {AppConfig} from "../values/structs/app-config";

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
