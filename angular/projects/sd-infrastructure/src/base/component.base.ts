/*组件基类*/
export abstract class ComponentBase {

    /*是否繁忙*/
    public isBusy: boolean = false;

    /**
     * 繁忙
     * */
    public busy(): void {
        this.isBusy = true;
    }

    /**
     * 空闲
     * */
    public idle(): void {
        this.isBusy = false;
    }
}
