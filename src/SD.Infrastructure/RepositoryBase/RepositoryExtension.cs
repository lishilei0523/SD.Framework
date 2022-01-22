using SD.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SD.Infrastructure.RepositoryBase
{
    /// <summary>
    /// 仓储扩展
    /// </summary>
    public static class RepositoryExtension
    {
        //Public

        #region # 分页 —— static IEnumerable<T> ToPage<T>(this IOrderedEnumerable<T> orderedResult...
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="orderedResult">已排序集合对象</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        public static IEnumerable<T> ToPage<T>(this IOrderedEnumerable<T> orderedResult, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            T[] array = orderedResult?.ToArray() ?? new T[0]; ;
            rowCount = array.Length;
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);

            return array.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 分页 —— static IQueryable<T> ToPage<T>(this IOrderedQueryable<T> orderedResult...
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="orderedResult">已排序集合对象</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>实体对象列表</returns>
        public static IQueryable<T> ToPage<T>(this IOrderedQueryable<T> orderedResult, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            rowCount = orderedResult.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);

            return orderedResult.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 构造实体过滤表达式 —— static Expression<Func<T, bool>> BuildFilterExpression<T>()
        /// <summary>
        /// 构造实体过滤表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>过滤表达式</returns>
        public static Expression<Func<T, bool>> BuildFilterExpression<T>() where T : PlainEntity
        {
            Expression<Func<T, bool>> defaultValue = x => true;
            Type type = typeof(T);
            ParameterExpression rootParam = Expression.Parameter(type, "x");

            #region # 聚合根处理方式

            if (type.IsSubclassOf(typeof(AggregateRootEntity)))
            {
                MemberExpression navMember = Expression.Property(rootParam, "Deleted");
                Expression notDeletedExpression = Expression.Equal(navMember, Expression.Constant(false));

                return Expression.Lambda<Func<T, bool>>(notDeletedExpression, rootParam);
            }

            #endregion

            #region # 普通实体处理方式

            //递归构造表达式
            HashSet<Expression> expressions = new HashSet<Expression>();
            BuildFilterExpressionRecursively(type, rootParam, expressions);

            Expression seedExpression = null;
            foreach (Expression expression in expressions)
            {
                seedExpression = seedExpression == null ? expression : Expression.AndAlso(seedExpression, expression);
            }
            if (seedExpression != null)
            {
                return Expression.Lambda<Func<T, bool>>(seedExpression, rootParam);
            }

            #endregion

            return defaultValue;
        }
        #endregion


        //Private

        #region # 递归构造普通实体过滤表达式 —— static void BuildFilterExpressionRecursively(Type type...
        /// <summary>
        /// 递归构造普通实体过滤表达式
        /// </summary>
        /// <param name="type">普通实体类型</param>
        /// <param name="propertyProvider">属性提供者表达式</param>
        /// <param name="expressions">表达式容器集合</param>
        private static void BuildFilterExpressionRecursively(Type type, Expression propertyProvider, HashSet<Expression> expressions)
        {
            //加载所有导航属性
            Func<PropertyInfo, bool> navPropertySelector =
                x =>
                    x.CanWrite &&
                    x.GetGetMethod() != null &&
                    x.GetGetMethod().IsVirtual &&
                    x.PropertyType.IsSubclassOf(typeof(PlainEntity));

            IEnumerable<PropertyInfo> navProperties = type.GetProperties().Where(navPropertySelector);

            foreach (PropertyInfo navProperty in navProperties)
            {
                Type navPropertyType = navProperty.PropertyType;

                //如果普通实体和普通实体有一对一关系，则忽略
                if (navPropertyType.IsSubclassOf(typeof(PlainEntity)) &&
                    !navPropertyType.IsSubclassOf(typeof(AggregateRootEntity)) &&
                    navPropertyType.GetProperties().Any(x => x.PropertyType == type))
                {
                    continue;
                }

                MemberExpression navMember = Expression.Property(propertyProvider, navProperty.Name);
                Expression navNotNullExpression = Expression.NotEqual(navMember, Expression.Constant(null));

                expressions.Add(navNotNullExpression);

                //如果是聚合根，增加聚合根未删除条件
                if (navPropertyType.IsSubclassOf(typeof(AggregateRootEntity)))
                {
                    MemberExpression deletedMember = Expression.Property(navMember, "Deleted");
                    Expression aggrNotDeletedExpression = Expression.Equal(deletedMember, Expression.Constant(false, deletedMember.Type));

                    expressions.Add(aggrNotDeletedExpression);
                }
                //如果导航属性类型是普通实体不是聚合根，递归处理
                if (navPropertyType.IsSubclassOf(typeof(PlainEntity)) && !navPropertyType.IsSubclassOf(typeof(AggregateRootEntity)))
                {
                    BuildFilterExpressionRecursively(navPropertyType, navMember, expressions);
                }
            }
        }
        #endregion
    }
}
