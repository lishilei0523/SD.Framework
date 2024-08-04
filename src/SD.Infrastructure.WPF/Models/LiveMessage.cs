using SD.Infrastructure.WPF.Enums;
using System;
using System.Windows.Media;

namespace SD.Infrastructure.WPF.Models
{
    /// <summary>
    /// 实时消息
    /// </summary>
    public class LiveMessage
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public LiveMessage() { }
        #endregion

        #region 01.创建实时消息构造器
        /// <summary>
        /// 创建实时消息构造器
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="messageType">消息类型</param>
        public LiveMessage(string content, LiveMessageType messageType)
            : this()
        {
            this.Content = content;
            this.MessageType = messageType;
            this.AddedTime = DateTime.Now;
            switch (messageType)
            {
                case LiveMessageType.Info:
                    this.Color = new SolidColorBrush(Colors.Black);
                    break;
                case LiveMessageType.Alert:
                    this.Color = new SolidColorBrush(Colors.Green);
                    break;
                case LiveMessageType.Warning:
                    this.Color = new SolidColorBrush(Colors.DarkOrange);
                    break;
                case LiveMessageType.Error:
                    this.Color = new SolidColorBrush(Colors.Red);
                    break;
                default:
                    this.Color = new SolidColorBrush(Colors.Black);
                    break;
            }
        }
        #endregion

        #endregion

        #region # 属性

        #region 内容 —— string Content
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        #endregion

        #region 颜色 —— SolidColorBrush Color
        /// <summary>
        /// 颜色
        /// </summary>
        public SolidColorBrush Color { get; set; }
        #endregion 

        #region 消息类型 —— LiveMessageType MessageType
        /// <summary>
        /// 消息类型
        /// </summary>
        public LiveMessageType MessageType { get; set; }
        #endregion

        #region 创建时间 —— DateTime AddedTime
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AddedTime { get; set; }
        #endregion

        #endregion
    }
}
