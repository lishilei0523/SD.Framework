using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 集合的扩展方法
    /// </summary>
    public static class CollectionExtension
    {
        #region # 遍历ForEach扩展方法 —— static void ForEach<T>(this IEnumerable<T> enumerable...
        /// <summary>
        /// 遍历ForEach扩展方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="enumerable">实现IEnumerable接口的类型</param>
        /// <param name="action">委托</param>
        /// <exception cref="ArgumentNullException">源集合对象为空、操作表达式为空</exception>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            #region # 验证参数

            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", string.Format("源{0}集合对象不可为空！", typeof(T).Name));
            }
            if (action == null)
            {
                throw new ArgumentNullException("action", @"操作表达式不可为空！");
            }

            #endregion

            foreach (T item in enumerable)
            {
                action(item);
            }
        }
        #endregion

        #region # 判断两个集合中的元素是否相等扩展方法 —— static bool EqualsTo<T>(this IEnumerable<T>...
        /// <summary>
        /// 判断两个集合中的元素是否相等扩展方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sourceList">源集合</param>
        /// <param name="targetList">目标集合</param>
        /// <returns>是否相等</returns>
        /// <exception cref="ArgumentNullException">源集合对象为空、目标集合对象为空</exception>
        public static bool EqualsTo<T>(this IEnumerable<T> sourceList, IEnumerable<T> targetList)
        {
            #region # 验证参数

            if (sourceList == null)
            {
                throw new ArgumentNullException("sourceList", string.Format(@"源{0}集合对象不可为空！", typeof(T).Name));
            }

            if (targetList == null)
            {
                throw new ArgumentNullException("targetList", string.Format(@"目标{0}集合对象不可为空！", typeof(T).Name));
            }

            #endregion

            //转数组
            sourceList = sourceList.ToArray();
            targetList = targetList.ToArray();

            #region 01.长度对比

            //长度不相等
            if (sourceList.Count() != targetList.Count())
            {
                return false;
            }

            //长度都为0
            if (!sourceList.Any() && !targetList.Any())
            {
                return true;
            }

            #endregion

            #region 02.浅度对比

            //元素对比
            if (!sourceList.Except(targetList).Any() && !targetList.Except(sourceList).Any())
            {
                return true;
            }

            #endregion

            #region 03.深度对比

            //将集合序列化为字符串
            string sourceListStr = sourceList.ToJson().Trim();
            string targetListStr = targetList.ToJson().Trim();

            //对比字符串是否相同
            if (sourceListStr == targetListStr)
            {
                return true;
            }

            #endregion

            return false;
        }
        #endregion

        #region # 判断两个字典中的元素是否相等扩展方法 —— static bool EqualsTo<TKey, TValue>(this IDictionary<TKey...
        /// <summary>
        /// 判断两个字典中的元素是否相等扩展方法
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="sourceDict">源字典</param>
        /// <param name="targetDict">目标字典</param>
        /// <returns>是否相等</returns>
        /// <exception cref="ArgumentNullException">源字典对象为空、目标字典对象为空</exception>
        public static bool EqualsTo<TKey, TValue>(this IDictionary<TKey, TValue> sourceDict, IDictionary<TKey, TValue> targetDict)
        {
            #region # 验证参数

            if (sourceDict == null)
            {
                throw new ArgumentNullException("sourceDict", "源字典对象不可为空！");
            }

            if (targetDict == null)
            {
                throw new ArgumentNullException("targetDict", "目标字典对象不可为空！");
            }

            #endregion

            #region 01.长度对比

            //长度不相等
            if (sourceDict.Count() != targetDict.Count())
            {
                return false;
            }

            //长度都为0
            if (!sourceDict.Any() && !targetDict.Any())
            {
                return true;
            }

            #endregion

            #region 02.深度对比

            IOrderedEnumerable<KeyValuePair<TKey, TValue>> sourceKeyValues = sourceDict.OrderBy(x => x.Key);
            IOrderedEnumerable<KeyValuePair<TKey, TValue>> targetKeyValues = targetDict.OrderBy(x => x.Key);

            //获取键集合
            IEnumerable<TKey> sourceKeys = sourceKeyValues.Select(x => x.Key);
            IEnumerable<TKey> targetKeys = targetKeyValues.Select(x => x.Key);

            //获取值集合
            IEnumerable<TValue> sourceValues = sourceKeyValues.Select(x => x.Value);
            IEnumerable<TValue> targetValues = targetKeyValues.Select(x => x.Value);

            //判断是否键值都相等
            if (sourceKeys.EqualsTo(targetKeys) && sourceValues.EqualsTo(targetValues))
            {
                return true;
            }

            #endregion

            return false;
        }
        #endregion

        #region # 判断集合是否为空或null —— static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        /// <summary>
        /// 判断集合是否为空或null
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="enumerable">集合对象</param>
        /// <returns>是否为空或null</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            return !enumerable.Any();
        }
        #endregion

        #region # 根据一个字段去重 —— static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable...
        /// <summary>
        /// 根据一个字段去重
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <typeparam name="TKey">去重的参照字段</typeparam>
        /// <param name="enumerable">源集合</param>
        /// <param name="keySelector">字段选择委托</param>
        /// <returns>去重后的集合</returns>
        /// <exception cref="ArgumentNullException">源集合对象为空、参照字段表达式为空</exception>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            #region # 验证参数

            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", string.Format("源{0}集合对象不可为空！", typeof(T).Name));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector", @"参照字段表达式不可为空！");
            }

            #endregion

            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return enumerable.Where(item => seenKeys.Add(keySelector(item)));
        }
        #endregion

        #region # AddRange扩展方法 —— static void AddRange<T>(this ICollection<T> collection...
        /// <summary>
        /// ICollection的AddRange扩展方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="enumerable">元素集合</param>
        /// <exception cref="ArgumentNullException">源集合对象为空、添加的集合项为空</exception>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
        {
            #region # 验证参数

            if (collection == null)
            {
                throw new ArgumentNullException("collection", string.Format("源{0}集合对象不可为空！", typeof(T).Name));
            }
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", string.Format("要添加的{0}集合项不可为空！", typeof(T).Name));
            }

            #endregion

            enumerable.ForEach(collection.Add);
        }
        #endregion

        #region # 分页扩展方法 —— static IEnumerable<T> FindByPage<T, TKeyOne, TKeyTwo>...
        /// <summary>
        /// 分页扩展方法
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <typeparam name="TKeyOne">排序键1</typeparam>
        /// <typeparam name="TKeyTwo">排序键2</typeparam>
        /// <param name="enumerable">集合对象</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelectorOne">排序键选择器1</param>
        /// <param name="keySelectorTwo">排序键选择器2</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>对象集合</returns>
        public static IEnumerable<T> FindByPage<T, TKeyOne, TKeyTwo>(this IEnumerable<T> enumerable, Func<T, bool> predicate, Func<T, TKeyOne> keySelectorOne, Func<T, TKeyTwo> keySelectorTwo, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            #region # 验证参数

            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", @"源集合对象不可为空！");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"查询条件对象不可为空！");
            }
            if (keySelectorOne == null || keySelectorTwo == null)
            {
                throw new ArgumentNullException("keySelector", @"参照字段表达式不可为空！");
            }
            if (pageIndex.IsZeroOrMinus())
            {
                throw new ArgumentOutOfRangeException("pageIndex", @"页码不可为0或负数！");
            }
            if (pageSize.IsZeroOrMinus())
            {
                throw new ArgumentOutOfRangeException("pageSize", @"页容量不可为0或负数！");
            }

            #endregion

            T[] list = enumerable.Where(predicate).ToArray();
            rowCount = list.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return list.OrderByDescending(keySelectorOne).ThenByDescending(keySelectorTwo).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 分页扩展方法 —— static IQueryable<T> FindByPage<T, TKeyOne, TKeyTwo>...
        /// <summary>
        /// 分页扩展方法
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <typeparam name="TKeyOne">排序键1</typeparam>
        /// <typeparam name="TKeyTwo">排序键2</typeparam>
        /// <param name="queryable">集合对象</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelectorOne">排序键选择器1</param>
        /// <param name="keySelectorTwo">排序键选择器2</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>对象集合</returns>
        public static IQueryable<T> FindByPage<T, TKeyOne, TKeyTwo>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, Expression<Func<T, TKeyOne>> keySelectorOne, Expression<Func<T, TKeyTwo>> keySelectorTwo, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            #region # 验证参数

            if (queryable == null)
            {
                throw new ArgumentNullException("queryable", @"源集合对象不可为空！");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"查询条件对象不可为空！");
            }
            if (keySelectorOne == null || keySelectorTwo == null)
            {
                throw new ArgumentNullException("keySelector", @"参照字段表达式不可为空！");
            }
            if (pageIndex.IsZeroOrMinus())
            {
                throw new ArgumentOutOfRangeException("pageIndex", @"页码不可为0或负数！");
            }
            if (pageSize.IsZeroOrMinus())
            {
                throw new ArgumentOutOfRangeException("pageSize", @"页容量不可为0或负数！");
            }

            #endregion

            IQueryable<T> list = queryable.Where(predicate);
            rowCount = list.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return list.OrderByDescending(keySelectorOne).ThenByDescending(keySelectorTwo).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # DataTable转换泛型集合扩展方法 —— static IList<T> ToList<T>(this DataTable...
        /// <summary>
        /// DataTable转换泛型集合扩展方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dataTable">数据表</param>
        /// <returns>泛型集合</returns>
        public static IList<T> ToList<T>(this DataTable dataTable)
        {
            //获取类型与属性信息
            Type currentType = typeof(T);
            PropertyInfo[] properties = currentType.GetProperties();

            //获取无参构造函数
            ConstructorInfo[] constructors = currentType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            ConstructorInfo noParamCtor = constructors.Single(x => x.GetParameters().Length == 0);

            #region # 验证

            if (dataTable == null)
            {
                throw new ArgumentNullException("dataTable", "数据表不可为null！");
            }

            #endregion

            IList<T> collection = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T instance = (T)noParamCtor.Invoke(null);

                foreach (PropertyInfo property in properties)
                {
                    if (dataTable.Columns.Contains(property.Name))
                    {
                        MethodInfo setter = property.GetSetMethod(true);
                        if (setter != null)
                        {
                            object value = row[property.Name] == DBNull.Value ? null : row[property.Name];
                            setter.Invoke(instance, new[] { value });
                        }
                    }
                }
                collection.Add(instance);
            }
            return collection;
        }
        #endregion

        #region # 泛型集合转换DataTable扩展方法 —— static DataTable ToDataTable<T>(this IEnumerable...
        /// <summary>
        /// 泛型集合转换DataTable扩展方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="enumerable">集合</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> enumerable)
        {
            #region # 验证参数

            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", "集合不可为null！");
            }

            #endregion

            //获取类型与属性信息
            Type currentType = typeof(T);
            PropertyInfo[] properties = currentType.GetProperties();

            DataTable dataTable = new DataTable();

            //创建列
            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(new DataColumn(property.Name));
            }

            //创建行
            T[] array = enumerable.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                foreach (PropertyInfo property in properties)
                {
                    dataTable.Rows[i][property.Name] = property.GetValue(array[i]);
                }
            }

            return dataTable;
        }
        #endregion

        #region # 泛型集合转换为分割字符串扩展方法 —— static string ToSplicString<T>(this IEnumerable<T>...
        /// <summary>
        /// 泛型集合转换为分割字符串扩展方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="enumerable">集合</param>
        /// <returns>分割字符串</returns>
        public static string ToSplicString<T>(this IEnumerable<T> enumerable)
        {
            StringBuilder builder = new StringBuilder();
            foreach (T item in enumerable)
            {
                builder.AppendFormat("'{0}'", item);
                builder.Append(',');
            }

            return builder.Length > 0 ? builder.ToString().Substring(0, builder.Length - 1) : string.Empty;
        }
        #endregion
    }
}
