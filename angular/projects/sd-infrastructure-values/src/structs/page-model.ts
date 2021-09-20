/*分页模型*/
export interface PageModel<T> {

    /*页码*/
    pageIndex: number;

    /*页容量*/
    pageSize: number;

    /*总记录数*/
    rowCount: number;

    /*总页数*/
    pageCount: number;

    /*数据集*/
    datas: Array<T>;
}
