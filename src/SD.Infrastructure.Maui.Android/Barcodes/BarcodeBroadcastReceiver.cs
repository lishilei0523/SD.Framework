using Android.App;
using Android.Content;
using System;

namespace SD.Infrastructure.Maui.Android.Barcodes
{
    /// <summary>
    /// 条码扫描广播接收器
    /// </summary>
    public sealed class BarcodeBroadcastReceiver : BroadcastReceiver
    {
        #region # 字段及构造器

        /// <summary>
        /// 接收条码事件
        /// </summary>
        public static Action<string> OnReceiveBarcode;

        /// <summary>
        /// 条码广播字段名称
        /// </summary>
        private static readonly string _BarcodeActionName;

        /// <summary>
        /// 条码数据字段名称
        /// </summary>
        private static readonly string _BarcodeExtraName;

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly BarcodeBroadcastReceiver _Instance;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static BarcodeBroadcastReceiver()
        {
            string barcodeActionName = BarcodeSetting.BarcodeActionName;
            string barcodeExtraName = BarcodeSetting.BarcodeExtraName;
            _BarcodeActionName = string.IsNullOrWhiteSpace(barcodeActionName)
                ? BarcodeConstants.DefaultBarcodeAction
                : barcodeActionName;
            _BarcodeExtraName = string.IsNullOrWhiteSpace(barcodeExtraName)
                ? BarcodeConstants.DefaultBarcodeExtra
                : barcodeExtraName;
            _Instance = new BarcodeBroadcastReceiver();
        }

        /// <summary>
        /// 私有构造器
        /// </summary>
        private BarcodeBroadcastReceiver()
        {

        }

        #endregion

        #region # 方法

        #region 接收广播事件 —— override void OnReceive(Context context, Intent intent)
        /// <summary>
        /// 接收广播事件
        /// </summary>
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent!.Action == _BarcodeActionName)
            {
                string barcodeNo = intent.GetStringExtra(_BarcodeExtraName);
                OnReceiveBarcode?.Invoke(barcodeNo);
            }
        }
        #endregion

        #region 注册广播接收器 —— static void Register(Activity activity)
        /// <summary>
        /// 注册广播接收器
        /// </summary>
        public static void Register(Activity activity)
        {
            IntentFilter filter = new IntentFilter(_BarcodeActionName);
            activity.RegisterReceiver(_Instance, filter);
        }
        #endregion

        #region 注销广播接收器 —— static void Unregister(Activity activity)
        /// <summary>
        /// 注销广播接收器
        /// </summary>
        public static void Unregister(Activity activity)
        {
            activity.UnregisterReceiver(_Instance);
        }
        #endregion 

        #endregion
    }
}
