import {Constants} from "./constants";
import {LoginInfo} from "../structs/login-info";
import {LoginMenuInfo} from "../structs/login-menu-info";
import {ApplicationType} from "../enums/application-type";

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

    /*登录菜单列表*/
    public static get loginMenus(): Array<LoginMenuInfo> {
        if (Membership.loginInfo) {
            let loginMenus = Membership.loginInfo.loginMenuInfos.filter(x => x.systemNo == "00" && x.applicationType == ApplicationType.IOS);
            Membership.filterLoginMenu(loginMenus, 1);

            return loginMenus;
        }
        return new Array<LoginMenuInfo>();
    }

    /*登录权限列表*/
    public static get loginAuthorityPaths(): Array<string> {
        if (Membership.loginInfo) {
            return Membership.loginInfo.loginAuthorityInfos.filter(x => x.systemNo == "00" && x.applicationType == ApplicationType.IOS).map(x => x.path);
        }
        return new Array<string>();
    }

    /**
     * 过滤用户菜单
     * */
    private static filterLoginMenu(loginMenus: LoginMenuInfo[], level: number): void {
        for (let loginMenu of loginMenus) {
            loginMenu.level = level;
            if (loginMenu.subMenuInfos.length > 0) {
                loginMenu.isLeaf = false;
                Membership.filterLoginMenu(loginMenu.subMenuInfos, level + 1);
            } else {
                loginMenu.isLeaf = true;
            }
        }
    }
}
