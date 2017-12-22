

using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 模型映射工具类
    /// </summary>
    /// <typeparam name="TSource">源实例类型</typeparam>
    /// <typeparam name="TTarget">目标实例类型</typeparam>
    public static class Transform<TSource, TTarget>
    {
        #region # 映射 —— static TTarget Map(TSource sourceInstance, Action<TSource, TTarget> beforeMa...
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="sourceInstance">源实例</param>
        /// <param name="beforeMapEventHandler">映射前事件处理者</param>
        /// <param name="afterMapEventHandler">映射后事件处理者</param>
        /// <param name="ignoreMembers">忽略映射成员集</param>
        /// <returns>目标实例</returns>
        public static TTarget Map(TSource sourceInstance, Action<TSource, TTarget> beforeMapEventHandler = null, Action<TSource, TTarget> afterMapEventHandler = null, params Expression<Func<TTarget, object>>[] ignoreMembers)
        {
            #region # 验证参数

            if (sourceInstance == null)
            {
                return default(TTarget);
            }

            #endregion

            MapperConfigurationExpression config = new MapperConfigurationExpression();
            IMappingExpression<TSource, TTarget> mapConfig = config.CreateMap<TSource, TTarget>();

            #region # 忽略映射成员处理

            foreach (Expression<Func<TTarget, object>> ignoreMember in ignoreMembers)
            {
                mapConfig.ForMember(ignoreMember, source => source.Ignore());
            }

            #endregion

            #region # 映射前后事件处理

            if (beforeMapEventHandler != null)
            {
                mapConfig.BeforeMap(beforeMapEventHandler);
            }
            if (afterMapEventHandler != null)
            {
                mapConfig.AfterMap(afterMapEventHandler);
            }

            #endregion

            IMapper mapper = new Mapper(new MapperConfiguration(config));
            return mapper.Map<TTarget>(sourceInstance);
        }
        #endregion

        #region # 填充 —— static void Fill(TSource sourceInstance, TTarget targetInstance)
        /// <summary>
        /// 将两个对象名称相同的属性替换赋值
        /// </summary>
        /// <param name="sourceInstance">源实例</param>
        /// <param name="targetInstance">目标实例</param>
        public static void Fill(TSource sourceInstance, TTarget targetInstance)
        {
            //01.获取源对象与目标对象的类型
            Type sourceType = sourceInstance.GetType();
            Type targetType = targetInstance.GetType();

            //02.获取源对象与目标对象的所有属性
            PropertyInfo[] sourceProps = sourceType.GetProperties();
            PropertyInfo[] targetProps = targetType.GetProperties();

            //03.双重遍历，判断属性名称是否相同，如果相同则赋值
            foreach (PropertyInfo tgtProp in targetProps)
            {
                foreach (PropertyInfo srcProp in sourceProps)
                {
                    if (tgtProp.Name == srcProp.Name)
                    {
                        tgtProp.SetValue(targetInstance, srcProp.GetValue(sourceInstance, null), null);
                    }
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 新模型映射工具类
    /// </summary>
    public static class Transform
    {
        #region # 映射 —— static TTarget Map<TSource, TTarget>(this TSource...
        /// <summary>
        /// 映射
        /// </summary>
        /// <typeparam name="TSource">源实例类型</typeparam>
        /// <typeparam name="TTarget">目标实例类型</typeparam>
        /// <param name="sourceInstance">源实例</param>
        /// <param name="beforeMapEventHandler">映射前事件处理者</param>
        /// <param name="afterMapEventHandler">映射后事件处理者</param>
        /// <param name="ignoreMembers">忽略映射成员集</param>
        /// <returns>目标实例</returns>
        public static TTarget Map<TSource, TTarget>(this TSource sourceInstance, Action<TSource, TTarget> beforeMapEventHandler = null, Action<TSource, TTarget> afterMapEventHandler = null, params Expression<Func<TTarget, object>>[] ignoreMembers)
        {
            return Transform<TSource, TTarget>.Map(sourceInstance, beforeMapEventHandler, afterMapEventHandler,
                ignoreMembers);
        }
        #endregion

        #region # 填充 —— static void Fill<TSource, TTarget>(this TSource...
        /// <summary>
        /// 将两个对象名称相同的属性替换赋值
        /// </summary>
        /// <typeparam name="TSource">源实例类型</typeparam>
        /// <typeparam name="TTarget">目标实例类型</typeparam>
        /// <param name="sourceInstance">源实例</param>
        /// <param name="targetInstance">目标实例</param>
        public static void Fill<TSource, TTarget>(this TSource sourceInstance, TTarget targetInstance)
        {
            Transform<TSource, TTarget>.Fill(sourceInstance, targetInstance);
        }
        #endregion
    }
}
