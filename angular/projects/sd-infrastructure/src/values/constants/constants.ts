import {AppConfig} from "../structs/app-config";

// @dynamic
/*常量*/
export class Constants {

    /*日期格式*/
    public static readonly dateFormat: string = "yyyy-MM-dd";

    /*日期时间格式*/
    public static readonly dateTimeFormat: string = "yyyy-MM-dd HH:mm:ss";

    /*时间格式*/
    public static readonly timeFormat: string = "HH:mm:ss";

    /*本地*/
    public static readonly locale: string = "zh-cn";

    /*当前登录用户键*/
    public static readonly keyOfCurrentUser: string = "CurrentUser";

    /*当前公钥键*/
    public static readonly keyOfPublicKey: string = "CurrentPublicKey";

    /*应用程序配置*/
    public static appConfig: AppConfig;
}
