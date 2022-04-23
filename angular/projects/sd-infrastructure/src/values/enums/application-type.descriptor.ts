import {Pipe, PipeTransform} from '@angular/core';
import {ApplicationType} from './application-type';

/*应用程序类型枚举描述器*/
@Pipe({
    name: 'applicationTypeDescriptor'
})
export class ApplicationTypeDescriptor implements PipeTransform {

    /**
     * 转换枚举描述
     * */
    public transform(applicationType: ApplicationType): string {
        switch (applicationType) {
            case ApplicationType.Web:
                return "Web应用程序";
            case ApplicationType.Windows:
                return "Windows应用程序";
            case ApplicationType.Android:
                return "Android应用程序";
            case ApplicationType.IOS:
                return "iOS应用程序";
            case ApplicationType.WindowsPhone:
                return "Windows Phone应用程序";
            case ApplicationType.Applet:
                return "小程序";
            case ApplicationType.Complex:
                return "复合应用程序";
        }
    }

    /**
     * 获取枚举描述字典
     * */
    public static getEnumMembers()
        : Set<{ key: ApplicationType, value: string }> {
        let enumMembers = new Set<{ key: ApplicationType; value: string }>();
        enumMembers.add({key: ApplicationType.Web, value: "Web应用程序"});
        enumMembers.add({key: ApplicationType.Windows, value: "Windows应用程序"});
        enumMembers.add({key: ApplicationType.Android, value: "Android应用程序"});
        enumMembers.add({key: ApplicationType.IOS, value: "iOS应用程序"});
        enumMembers.add({key: ApplicationType.WindowsPhone, value: "Windows Phone应用程序"});
        enumMembers.add({key: ApplicationType.Applet, value: "小程序"});
        enumMembers.add({key: ApplicationType.Complex, value: "复合应用程序"});

        return enumMembers;
    }
}
