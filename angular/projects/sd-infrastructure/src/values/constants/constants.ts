import {AppConfig} from "../structs/app-config";

// @dynamic
/*常量*/
export class Constants {

    /*空字符串*/
    public static readonly stringEmpty: string = "";

    /*int最大值*/
    public static readonly intMaxValue: number = 2147483647;

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

    /*当前登录用户菜单列表键*/
    public static readonly keyOfCurrentUserMenus: string = "CurrentUserMenus";

    /*当前登录用户权限路径列表键*/
    public static readonly keyOfCurrentUserAuthorityPaths: string = "CurrentUserAuthorityPaths";

    /*当前公钥键*/
    public static readonly keyOfPublicKey: string = "PublicKey";

    /*应用程序配置*/
    public static appConfig: AppConfig;
}
