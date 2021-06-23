using SD.Infrastructure.MVC.Server;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("7fdc886e-84a0-4225-bdcf-eddf9ca6c20a")]

// 注射至MVC应用程序
[assembly: PreApplicationStartMethod(typeof(PreAppStart), nameof(PreAppStart.Initialize))]