import {ApplicationType} from "../enums/application-type";

/*登录菜单信息*/
export interface LoginMenuInfo {

    /*信息系统编号*/
    systemNo: string;

    /*应用程序类型*/
    applicationType: ApplicationType;

    /*菜单Id*/
    id: string;

    /*上级菜单Id*/
    parentId: string | null;

    /*菜单名称*/
    name: string;

    /*菜单层次*/
    level: number;

    /*菜单图标*/
    icon: string;

    /*链接*/
    url: string;

    /*路径*/
    path: string;

    /*菜单排序*/
    sort: number;

    /*是否是叶子级节点*/
    isLeaf: boolean;

    /*下级菜单列表*/
    subMenuInfos: Array<LoginMenuInfo>;
}
