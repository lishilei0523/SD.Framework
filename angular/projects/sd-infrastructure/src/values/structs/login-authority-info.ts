import {ApplicationType} from "../enums/application-type";

/*登录权限信息*/
export interface LoginAuthorityInfo {

    /*信息系统编号*/
    systemNo: string;

    /*应用程序类型*/
    applicationType: ApplicationType;

    /*权限Id*/
    id: string;

    /*权限名称*/
    name: string;

    /*权限路径*/
    path: string;

    /*英文名称*/
    englishName: string;
}
