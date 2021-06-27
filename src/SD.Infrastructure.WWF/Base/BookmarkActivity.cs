using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SD.Infrastructure.WWF.Base
{
    /// <summary>
    /// 工作流书签基类
    /// </summary>
    public abstract class BookmarkActivity : NativeActivity
    {
        #region # 属性

        #region 是否使工作流进入空闲状态 —— override bool CanInduceIdle
        /// <summary>
        /// 是否使工作流进入空闲状态
        /// </summary>
        protected override bool CanInduceIdle
        {
            get { return true; }
        }
        #endregion

        #endregion

        #region # 方法

        //Public

        #region 执行创建书签 —— override void Execute(NativeActivityContext context)
        /// <summary>
        /// 执行创建书签
        /// </summary>
        /// <param name="context">活动上下文</param>
        protected override void Execute(NativeActivityContext context)
        {
            Type bookmarkType = this.GetType();
            string bookmarkTypeName = bookmarkType.FullName;

            context.CreateBookmark(bookmarkTypeName, this.ResumeCallback);
        }
        #endregion

        #region 书签恢复时的回调方法 —— virtual void ResumeCallback(NativeActivityContext context...
        /// <summary>
        /// 书签恢复时的回调方法
        /// </summary>
        /// <param name="context">活动上下文</param>
        /// <param name="bookmark">书签实例</param>
        /// <param name="bookmarkParameter">书签参数</param>
        protected virtual void ResumeCallback(NativeActivityContext context, Bookmark bookmark, object bookmarkParameter)
        {
            //填充参数值
            this.FillArgumentValue(context, bookmarkParameter);
        }
        #endregion


        //Private

        #region 填充参数值 —— void FillArgumentValue(NativeActivityContext context...
        /// <summary>
        /// 填充参数值
        /// </summary>
        /// <param name="context">活动上下文</param>
        /// <param name="bookmarkParameter">书签参数</param>
        protected void FillArgumentValue(NativeActivityContext context, object bookmarkParameter)
        {
            if (bookmarkParameter != null)
            {
                #region # 验证

                if (!(bookmarkParameter is IDictionary<string, object> parameters))
                {
                    throw new ArgumentOutOfRangeException(nameof(bookmarkParameter), "参数类型不正确，必须是\"IDictionary<string, object>\"类型！");
                }

                #endregion

                Type bookmarkType = this.GetType();
                Type argumentType = typeof(Argument);

                PropertyInfo[] properties = bookmarkType.GetProperties();
                IEnumerable<PropertyInfo> argumentProperties = properties.Where(x => x.PropertyType.IsSubclassOf(argumentType));

                foreach (PropertyInfo property in argumentProperties)
                {
                    //获取参数实例
                    Argument argumentInstance = (Argument)property.GetValue(this);

                    //验证
                    if (!parameters.TryGetValue(property.Name, out object value))
                    {
                        throw new ArgumentOutOfRangeException(nameof(bookmarkParameter), $"参数\"{property.Name}\"未赋值！");
                    }

                    context.SetValue(argumentInstance, value);
                }
            }
        }
        #endregion

        #endregion
    }
}
