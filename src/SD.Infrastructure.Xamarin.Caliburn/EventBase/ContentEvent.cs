namespace SD.Infrastructure.Xamarin.Caliburn.EventBase
{
    /// <summary>
    /// 内容事件
    /// </summary>
    public class ContentEvent
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ContentEvent() { }
        #endregion

        #region 01.创建内容事件构造器
        /// <summary>
        /// 创建内容事件构造器
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public ContentEvent(string title, string content = null)
            : this()
        {
            this.Title = title;
            this.Content = content;
        }
        #endregion 

        #endregion

        #region # 属性

        #region 标题 —— string Title
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        #endregion

        #region 内容 —— string Content
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        #endregion 

        #endregion
    }
}
