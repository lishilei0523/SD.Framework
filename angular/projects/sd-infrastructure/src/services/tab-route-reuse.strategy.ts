import {ActivatedRouteSnapshot, DetachedRouteHandle, RouteReuseStrategy} from '@angular/router';

/*选项卡路由重用策略*/
export class TabRouteReuseStrategy extends RouteReuseStrategy {

    /*路由Handle字典*/
    private static routeHandles: { [key: string]: DetachedRouteHandle } = {};

    /**
     * 删除路由快照
     * */
    public static removeSnapshot(url: string): void {
        delete TabRouteReuseStrategy.routeHandles[url];
    }

    /**
     * 是否应卸载路由
     * */
    public shouldDetach(route: ActivatedRouteSnapshot): boolean {
        if (route?.routeConfig?.data?.reuse == false) {
            return false;
        }
        return true;
    }

    /**
     * 是否应附加路由
     * */
    public shouldAttach(route: ActivatedRouteSnapshot): boolean {
        let url = TabRouteReuseStrategy.getRouteUrl(route);
        if (url) {
            let handle: DetachedRouteHandle = TabRouteReuseStrategy.routeHandles[url];
            if (handle) {
                return true;
            } else {
                return false;
            }
        }

        return false;
    }

    /**
     * 是否应复用路由
     * */
    public shouldReuseRoute(future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean {
        if (future.routeConfig === curr.routeConfig) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * 缓存路由快照
     * */
    public store(route: ActivatedRouteSnapshot, handle: DetachedRouteHandle): void {
        let url = TabRouteReuseStrategy.getRouteUrl(route);
        if (url) {
            TabRouteReuseStrategy.routeHandles[url] = handle
        }
    }

    /**
     * 获取路由快照
     * */
    public retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle | null {
        let url = TabRouteReuseStrategy.getRouteUrl(route);
        if (url) {
            return TabRouteReuseStrategy.routeHandles[url];
        }

        return null;
    }

    /**
     * 获取路由快照URL
     * */
    private static getRouteUrl(route: ActivatedRouteSnapshot): string | null {
        let routSnapshot: any = route;

        if (!routSnapshot) {
            return null;
        }
        if (!routSnapshot._routerState) {
            return null;
        }
        if (!routSnapshot._routerState.url) {
            return null;
        }

        return routSnapshot._routerState.url;
    }
}
