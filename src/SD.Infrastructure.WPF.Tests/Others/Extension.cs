using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.Infrastructure.WPF.Tests.Others
{
    public static class Extension
    {

        public static IEnumerable<T> ToPage<T>(this IEnumerable<T> enumerable, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            #region # 验证参数

            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", @"源集合对象不可为空！");
            }

            #endregion

            T[] list = enumerable.ToArray();
            rowCount = list.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }


    }
}
