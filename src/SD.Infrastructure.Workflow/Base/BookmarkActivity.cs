using System.Activities;

namespace SD.Infrastructure.Workflow.Base
{
    /// <summary>
    /// 工作流书签基类
    /// </summary>
    public abstract class BookmarkActivity : NativeActivity
    {
        /// <summary>
        /// 书签名称
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 是否使工作流进入空闲状态
        /// </summary>
        protected override bool CanInduceIdle
        {
            get { return true; }
        }


        /// <summary>
        /// 执行创建书签
        /// </summary>
        /// <param name="context">活动上下文</param>
        protected override void Execute(NativeActivityContext context)
        {
            context.CreateBookmark(this.Name, this.ResumeCallback);
        }

        /// <summary>
        /// 书签恢复时的回调方法
        /// </summary>
        /// <param name="context">活动上下文</param>
        /// <param name="bookmark">书签实例</param>
        /// <param name="parameter">参数</param>
        protected abstract void ResumeCallback(NativeActivityContext context, Bookmark bookmark, dynamic parameter);
    }
}
