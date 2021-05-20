using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Markup;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("SD.Infrastructure.WPF")]
[assembly: AssemblyDescription("SD.Framework 基础设施 - WPF基础")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SD")]
[assembly: AssemblyProduct("SD.Infrastructure.WPF")]
[assembly: AssemblyCopyright("Copyright © SD 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("8a7ccb16-13c5-4778-ac33-2d185b40dc6c")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
//可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
//通过使用 "*"，如下所示:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


//XAML命名空间映射
[assembly: XmlnsPrefix("https://github.com/lishilei0523/SD.Framework", "sd")]
[assembly: XmlnsDefinition("https://github.com/lishilei0523/SD.Framework", "SD.Infrastructure.WPF.Attachers")]
[assembly: XmlnsDefinition("https://github.com/lishilei0523/SD.Framework", "SD.Infrastructure.WPF.Converters")]
[assembly: XmlnsDefinition("https://github.com/lishilei0523/SD.Framework", "SD.Infrastructure.WPF.CustomControls")]
[assembly: XmlnsDefinition("https://github.com/lishilei0523/SD.Framework", "SD.Infrastructure.WPF.UserControls")]
