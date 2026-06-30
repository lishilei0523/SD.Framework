using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using FluentAvalonia.UI.Controls;
using IconPacks.Avalonia.MaterialDesign;
using SD.Infrastructure.Avalonia.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Infrastructure.Avalonia.CustomControls
{
    /// <summary>
    /// 消息框
    /// </summary>
    public static class MessageBox
    {
        #region # 显示消息 —— static async Task<FATaskDialogStandardResult> Show(string content...
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="title">消息标题</param>
        /// <param name="button">按钮</param>
        /// <param name="icon">图标</param>
        /// <returns>消息结果</returns>
        public static async Task<FATaskDialogStandardResult> Show(string content, string title = null, MessageBoxButton button = MessageBoxButton.OK, PackIconMaterialDesignKind icon = PackIconMaterialDesignKind.Info)
        {
            FATaskDialog dialog = new FATaskDialog
            {
                Title = title,
                Content = content,
                ShowProgressBar = false,
                FooterVisibility = FATaskDialogFooterVisibility.Never
            };

            PackIconMaterialDesign packIcon = new PackIconMaterialDesign { Kind = PackIconMaterialDesignKind.Info };
            dialog.IconSource = new FAPathIconSource
            {
                Data = packIcon.Data
            };

            FATaskDialogButton okButton = FATaskDialogButton.OKButton;
            FATaskDialogButton cancelButton = FATaskDialogButton.CancelButton;
            FATaskDialogButton yesButton = FATaskDialogButton.YesButton;
            FATaskDialogButton noButton = FATaskDialogButton.NoButton;
            okButton.Text = "确定";
            cancelButton.Text = "取消";
            yesButton.Text = "是";
            noButton.Text = "否";

            switch (button)
            {
                case MessageBoxButton.OK:
                    dialog.Buttons.Add(okButton);
                    break;
                case MessageBoxButton.OKCancel:
                    dialog.Buttons.Add(okButton);
                    dialog.Buttons.Add(cancelButton);
                    break;
                case MessageBoxButton.YesNoCancel:
                    dialog.Buttons.Add(yesButton);
                    dialog.Buttons.Add(noButton);
                    dialog.Buttons.Add(cancelButton);
                    break;
                case MessageBoxButton.YesNo:
                    dialog.Buttons.Add(yesButton);
                    dialog.Buttons.Add(noButton);
                    break;
                default:
                    dialog.Buttons.Add(okButton);
                    break;
            }

            IClassicDesktopStyleApplicationLifetime lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime;
            dialog.XamlRoot = lifetime!.Windows.Any(window => window.IsActive)
                ? lifetime.Windows.Single(window => window.IsActive)
                : lifetime.MainWindow;

            FATaskDialogStandardResult result = (FATaskDialogStandardResult)await dialog.ShowAsync();

            return result;
        }
        #endregion
    }
}
