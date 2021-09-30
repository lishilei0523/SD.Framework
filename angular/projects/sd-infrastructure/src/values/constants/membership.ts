import {Constants} from "./constants";
import {LoginInfo} from "../structs/login-info";
import {LoginMenuInfo} from "../structs/login-menu-info";

// @dynamic
/*用户信息*/
export class Membership {

    /*登录信息 - Getter*/
    public static get loginInfo(): LoginInfo | null {
        let json = localStorage.getItem(Constants.keyOfCurrentUser);
        if (json) {
            return JSON.parse(json);
        } else {
            return null;
        }
    }

    /*登录信息 - Setter*/
    public static set loginInfo(value: LoginInfo | null) {
        if (value) {
            localStorage.setItem(Constants.keyOfCurrentUser, JSON.stringify(value));
        } else {
            localStorage.removeItem(Constants.keyOfCurrentUser);
        }
    }

    /*登录菜单列表 - Getter*/
    public static get loginMenus(): Array<LoginMenuInfo> | null {
        let json = localStorage.getItem(Constants.keyOfCurrentUserMenus);
        if (json) {
            return JSON.parse(json);
        } else {
            return null;
        }
    }

    /*登录菜单列表 - Setter*/
    public static set loginMenus(value: Array<LoginMenuInfo> | null) {
        if (value) {
            localStorage.setItem(Constants.keyOfCurrentUserMenus, JSON.stringify(value));
        } else {
            localStorage.removeItem(Constants.keyOfCurrentUserMenus);
        }
    }

    /*登录权限路径列表 - Getter*/
    public static get loginAuthorityPaths(): Array<string> | null {
        let json = localStorage.getItem(Constants.keyOfCurrentUserAuthorityPaths);
        if (json) {
            return JSON.parse(json);
        } else {
            return null;
        }
    }

    /*登录权限路径列表 - Setter*/
    public static set loginAuthorityPaths(value: Array<string> | null) {
        if (value) {
            localStorage.setItem(Constants.keyOfCurrentUserAuthorityPaths, JSON.stringify(value));
        } else {
            localStorage.removeItem(Constants.keyOfCurrentUserAuthorityPaths);
        }
    }

    /**
     * 注销登录
     * */
    public static logout(): void {
        Membership.loginInfo = null;
        Membership.loginMenus = null;
        Membership.loginAuthorityPaths = null;
    }
}
