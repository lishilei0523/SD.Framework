using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace SD.Infrastructure.WPF.Extensions
{
    /// <summary>
    /// Bitmap扩展
    /// </summary>
    public static class BitmapExtension
    {
        #region # Bitmap转换BitmapImage —— static BitmapImage ToBitmapImage(this Bitmap bitmap)
        /// <summary>
        /// Bitmap转换BitmapImage
        /// </summary>
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, bitmap.RawFormat);
            stream.Seek(0, SeekOrigin.Begin);

            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
        #endregion
    }
}
