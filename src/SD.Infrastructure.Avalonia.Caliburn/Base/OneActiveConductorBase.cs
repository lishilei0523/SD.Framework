using Caliburn.Micro;
using SD.Infrastructure.Avalonia.Caliburn.Aspects;
using SD.Infrastructure.Avalonia.Enums;
using SD.Infrastructure.Avalonia.Interfaces;
using SD.Infrastructure.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Infrastructure.Avalonia.Caliburn.Base
{
    /// <summary>
    /// 单活动Conductor基类
    /// </summary>
    public abstract class OneActiveConductorBase : Conductor<IScreen>.Collection.OneActive, IBusy
    {
        #region # 字段及构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected OneActiveConductorBase()
        {
            //默认值
            this.LiveMessages = new ObservableCollection<LiveMessage>();
        }

        #endregion

        #region # 属性

        #region 是否繁忙 —— bool IsBusy
        /// <summary>
        /// 是否繁忙
        /// </summary>
        [DependencyProperty]
        public bool IsBusy { get; set; }
        #endregion

        #region 实时消息列表 —— ObservableCollection<LiveMessage> LiveMessages
        /// <summary>
        /// 实时消息列表
        /// </summary>
        [DependencyProperty]
        public ObservableCollection<LiveMessage> LiveMessages { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 挂起繁忙状态 —— void Busy()
        /// <summary>
        /// 挂起繁忙状态
        /// </summary>
        public void Busy()
        {
            this.IsBusy = true;
        }
        #endregion

        #region 释放繁忙状态 —— void Idle()
        /// <summary>
        /// 释放繁忙状态
        /// </summary>
        public void Idle()
        {
            this.IsBusy = false;
        }
        #endregion

        #region 发送实时消息 —— void SendMessage(string content...
        /// <summary>
        /// 发送实时消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="messageType">消息类型</param>
        public void SendMessage(string content, LiveMessageType messageType)
        {
            this.OnUIThread(() =>
            {
                LiveMessage liveMessage = new LiveMessage(content, messageType);
                this.LiveMessages.Insert(0, liveMessage);
            });
        }
        #endregion

        #region 清空实时消息 —— void ClearMessages()
        /// <summary>
        /// 清空实时消息
        /// </summary>
        public void ClearMessages()
        {
            this.OnUIThread(() =>
            {
                this.LiveMessages.Clear();
            });
        }
        #endregion

        #region 导出实时消息 —— void ExportMessages()
        /// <summary>
        /// 导出实时消息
        /// </summary>
        public void ExportMessages()
        {
            IList<string> messages = new List<string>();
            foreach (LiveMessage liveMessage in this.LiveMessages.ToArray())
            {
                messages.Add($"{liveMessage.Content}\t{liveMessage.MessageType.ToString()}\t{liveMessage.AddedTime:yyyy-MM-dd HH:mm:ss.fff}");
            }

            //TODO 导出实时消息
            //SaveFileDialog saveFileDialog = new SaveFileDialog
            //{
            //    Filter = "(*.txt)|*.txt",
            //    FileName = $"实时消息_{DateTime.Now:yyyyMMddHHmmss}",
            //    AddExtension = true,
            //    RestoreDirectory = true
            //};
            //if (saveFileDialog.ShowDialog() == true)
            //{
            //    string filePath = saveFileDialog.FileName;
            //    File.WriteAllLines(filePath, messages);
            //}
        }
        #endregion

        #region 在UI线程执行 —— void OnUIThread(Action action)
        /// <summary>
        /// 在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public new void OnUIThread(System.Action action)
        {
            action.OnUIThread();
        }
        #endregion

        #region 异步在UI线程执行 —— void BeginOnUIThread(Action action)
        /// <summary>
        /// 异步在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public void BeginOnUIThread(System.Action action)
        {
            action.BeginOnUIThread();
        }
        #endregion

        #region 异步等待在UI线程执行 —— Task OnUIThreadAsync(Func<Task> action)
#if NET461 || NET462 || NETCOREAPP3_1_OR_GREATER
        /// <summary>
        /// 异步等待在UI线程执行
        /// </summary>
        /// <param name="action">方法</param>
        public Task OnUIThreadAsync(Func<Task> action)
        {
            return action.OnUIThreadAsync();
        }
#endif
        #endregion

        #endregion
    }
}
