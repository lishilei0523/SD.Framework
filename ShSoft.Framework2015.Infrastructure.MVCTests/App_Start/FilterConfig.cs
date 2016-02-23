using System.Web;
using System.Web.Mvc;

namespace ShSoft.Framework2015.Infrastructure.MVCTests
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}