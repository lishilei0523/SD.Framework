/*选项卡*/
export class Tab {

    /*菜单Id*/
    public menuId: string;

    /*菜单名称*/
    public menuName: string;

    /*链接地址*/
    public menuUrl: string;

    /*索引*/
    public index: number;

    /*是否选中*/
    public selected: boolean;

    /**
     * 创建选项卡构造器
     * */
    public constructor(menuId: string, menuName: string, menuUrl: string, index: number, selected: boolean) {
        this.menuId = menuId;
        this.menuName = menuName;
        this.menuUrl = menuUrl;
        this.index = index;
        this.selected = selected;
    }
}
