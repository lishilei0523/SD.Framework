using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace SD.Infrastructure.Maui.TriggerActions
{
    /// <summary>
    /// 展示密码触发器行为
    /// </summary>
    public class ShowPasswordTriggerAction : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 展示图标
        /// </summary>
        public string ShowIcon { get; set; }

        /// <summary>
        /// 隐藏图标
        /// </summary>
        public string HideIcon { get; set; }

        /// <summary>
        /// 是否隐藏密码
        /// </summary>
        private bool _hidePassword = true;

        /// <summary>
        /// 是否隐藏密码
        /// </summary>
        public bool HidePassword
        {
            set
            {
                if (this._hidePassword != value)
                {
                    this._hidePassword = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.HidePassword)));
                }
            }
            get => this._hidePassword;
        }

        /// <summary>
        /// 执行
        /// </summary>
        protected override void Invoke(ImageButton sender)
        {
            sender.Source = this.HidePassword ? this.ShowIcon : this.HideIcon;
            this.HidePassword = !this.HidePassword;
        }
    }
}
