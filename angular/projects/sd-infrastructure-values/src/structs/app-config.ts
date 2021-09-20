/*应用程序配置*/
export interface AppConfig {

    /*WebApi地址前缀*/
    webApiPrefix: string;

    /*身份认证是否启用*/
    authenticationEnabled: boolean;

    /*授权是否启用*/
    authorizationEnabled: boolean;
}
