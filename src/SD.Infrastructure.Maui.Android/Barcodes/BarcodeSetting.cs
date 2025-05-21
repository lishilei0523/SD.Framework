using Microsoft.Maui.Storage;

namespace SD.Infrastructure.Maui.Android.Barcodes
{
    /// <summary>
    /// 条码扫描设置
    /// </summary>
    public static class BarcodeSetting
    {
        /// <summary>
        /// 条码广播字段名称
        /// </summary>
        public static string BarcodeActionName
        {
            get => Preferences.Get(BarcodeConstants.BarcodeActionName, null);
            set => Preferences.Set(BarcodeConstants.BarcodeActionName, value);
        }

        /// <summary>
        /// 条码数据字段名称
        /// </summary>
        public static string BarcodeExtraName
        {
            get => Preferences.Get(BarcodeConstants.BarcodeExtraName, null);
            set => Preferences.Set(BarcodeConstants.BarcodeExtraName, value);
        }
    }
}
