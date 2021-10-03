/*NG-ZORRO树节点*/
export class NzNode {

    /**
     * 创建NG-ZORRO树节点构造器
     * @param key - 键
     * @param title - 标题
     * @param isLeaf - 是否叶子级
     * @param icon - 图标
     * @param author - 作者
     * */
    public constructor(key: string, title: string, isLeaf: boolean, icon?: string, author?: string) {
        this.key = key;
        this.title = title;
        this.expanded = true;
        this.disabled = false;
        this.isLeaf = isLeaf;
        this.icon = icon;
        this.author = author;
        this.children = new Array<NzNode>();
    }

    /*索引器*/
    [key: string]: any;

    /*键*/
    public key: string;

    /*标题*/
    public title: string;

    /*是否展开*/
    public expanded: boolean;

    /*是否停用*/
    public disabled: boolean;

    /*是否叶子级*/
    public isLeaf: boolean;

    /*是否勾选*/
    public checked?: boolean;

    /*是否选中*/
    public selected?: boolean;

    /*是否可选中*/
    public selectable?: boolean;

    /*是否停用复选框*/
    public disableCheckbox?: boolean;

    /*图标*/
    public icon?: string;

    /*作者*/
    public author?: string;

    /*下级节点集*/
    public children: Array<NzNode>;
}
