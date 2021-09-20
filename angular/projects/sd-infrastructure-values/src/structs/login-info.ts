import {LoginSystemInfo} from "./login-system-info";
import {LoginMenuInfo} from "./login-menu-info";
import {LoginAuthorityInfo} from "./login-authority-info";

/*登录信息*/
export interface LoginInfo {

    /*用户名*/
    loginId: string;

    /*真实姓名*/
    realName: string;

    /*公钥*/
    publicKey: string;

    /*信息系统列表*/
    loginSystemInfos: Array<LoginSystemInfo>;

    /*菜单列表*/
    loginMenuInfos: Array<LoginMenuInfo>;

    /*权限列表*/
    loginAuthorityInfos: Array<LoginAuthorityInfo>;
}
