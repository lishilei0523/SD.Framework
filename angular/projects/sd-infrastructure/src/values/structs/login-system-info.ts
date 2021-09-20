import {ApplicationType} from "../enums/application-type";

/*登录信息系统信息*/
export interface LoginSystemInfo {

    /*信息系统编号*/
    number: string;

    /*信息系统名称*/
    name: string;

    /*应用程序类型*/
    applicationType: ApplicationType;

    /*首页*/
    index: string;
}
