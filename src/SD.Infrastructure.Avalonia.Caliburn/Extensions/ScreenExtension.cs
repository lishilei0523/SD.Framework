using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Infrastructure.Avalonia.Caliburn.Extensions
{
    /// <summary>
    /// 视图模型扩展
    /// </summary>
    public static class ScreenExtension
    {
        #region # 打开文件 —— static IReadOnlyList<IStorageFile> OpenFilePickerAsync(this Screen screen...
        /// <summary>
        /// 打开文件
        /// </summary>
        public static async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(this Screen screen, FilePickerOpenOptions options)
        {
            Control view = (Control)screen.GetView();
            TopLevel topLevel = TopLevel.GetTopLevel(view);
            IReadOnlyList<IStorageFile> storageFiles = await topLevel!.StorageProvider.OpenFilePickerAsync(options);

            return storageFiles;
        }
        #endregion

        #region # 打开文件夹 —— static IReadOnlyList<IStorageFolder> OpenFolderPickerAsync(this Screen screen...
        /// <summary>
        /// 打开文件夹
        /// </summary>
        public static async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(this Screen screen, FolderPickerOpenOptions options)
        {
            Control view = (Control)screen.GetView();
            TopLevel topLevel = TopLevel.GetTopLevel(view);
            IReadOnlyList<IStorageFolder> storageFolders = await topLevel!.StorageProvider.OpenFolderPickerAsync(options);

            return storageFolders;
        }
        #endregion

        #region # 保存文件 —— static IStorageFile SaveFilePickerAsync(this Screen screen...
        /// <summary>
        /// 保存文件
        /// </summary>
        public static async Task<IStorageFile> SaveFilePickerAsync(this Screen screen, FilePickerSaveOptions options)
        {
            Control view = (Control)screen.GetView();
            TopLevel topLevel = TopLevel.GetTopLevel(view);
            IStorageFile storageFile = await topLevel!.StorageProvider.SaveFilePickerAsync(options);

            return storageFile;
        }
        #endregion
    }
}
