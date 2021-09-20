import {AppConfigService} from "./app-config.service";

// @dynamic
/**
 * 初始化应用程序配置函数
 * @param appConfigService - 应用程序配置服务
 * */
export function InitializeAppConfig(appConfigService: AppConfigService) {
    const result = () => appConfigService.loadAppConfig();
    return result;
}
