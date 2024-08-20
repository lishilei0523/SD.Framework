using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.Attachers
{
    /// <summary>
    /// PasswordBox附加器
    /// </summary>
    public static class PasswordBoxAttacher
    {
        #region # 字段及构造器

        /// <summary>
        /// 密码依赖属性
        /// </summary>
        public static readonly DependencyProperty PasswordProperty;

        /// <summary>
        /// 是否已附加依赖属性
        /// </summary>
        public static readonly DependencyProperty IsAttachedProperty;

        /// <summary>
        /// 是否正在更新依赖属性
        /// </summary>
        public static readonly DependencyProperty IsUpdatingProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static PasswordBoxAttacher()
        {
            //注册依赖属性
            PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxAttacher), new FrameworkPropertyMetadata(string.Empty, OnPasswordChanged));
            IsAttachedProperty = DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(PasswordBoxAttacher), new PropertyMetadata(false, OnAttachedChanged));
            IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxAttacher));
        }

        #endregion

        #region # 属性

        #region 获取密码 —— static string GetPassword(DependencyObject dependencyObject)
        /// <summary>
        /// 获取密码
        /// </summary>
        public static string GetPassword(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(PasswordProperty);
        }
        #endregion

        #region 设置密码 —— static void SetPassword(DependencyObject dependencyObject...
        /// <summary>
        /// 设置密码
        /// </summary>
        public static void SetPassword(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(PasswordProperty, value);
        }
        #endregion

        #region 获取是否已附加 —— static bool GetIsAttached(DependencyObject dependencyObject)
        /// <summary>
        /// 获取是否已附加
        /// </summary>
        public static bool GetIsAttached(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsAttachedProperty);
        }
        #endregion

        #region 设置是否已附加 —— static void SetIsAttached(DependencyObject dependencyObject...
        /// <summary>
        /// 设置是否已附加
        /// </summary>
        public static void SetIsAttached(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsAttachedProperty, value);
        }
        #endregion

        #region 获取是否正在更新 —— static bool GetIsUpdating(DependencyObject dependencyObject)
        /// <summary>
        /// 获取是否正在更新
        /// </summary>
        private static bool GetIsUpdating(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsUpdatingProperty);
        }
        #endregion

        #region 设置是否正在更新 —— static void SetIsUpdating(DependencyObject dependencyObject...
        /// <summary>
        /// 设置是否正在更新
        /// </summary>
        private static void SetIsUpdating(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsUpdatingProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 密码改变回调方法 —— static void OnPasswordChanged(DependencyObject sender...
        /// <summary>
        /// 密码改变回调方法
        /// </summary>
        private static void OnPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            passwordBox.PasswordChanged -= PasswordChangedEventHandler;

            if (!GetIsUpdating(passwordBox))
            {
                passwordBox.Password = eventArgs.NewValue?.ToString() ?? string.Empty;
            }

            passwordBox.PasswordChanged += PasswordChangedEventHandler;
        }
        #endregion

        #region 是否已附加改变回调方法 —— static void OnAttachedChanged(DependencyObject sender...
        /// <summary>
        /// 是否已附加改变回调方法
        /// </summary>
        private static void OnAttachedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            if ((bool)eventArgs.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChangedEventHandler;
            }
            if ((bool)eventArgs.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChangedEventHandler;
            }
        }
        #endregion

        #region 密码改变事件处理程序 —— static void PasswordChangedEventHandler(object sender...
        /// <summary>
        /// 密码改变事件处理程序
        /// </summary>
        private static void PasswordChangedEventHandler(object sender, RoutedEventArgs eventArgs)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
        #endregion

        #endregion
    }
}
