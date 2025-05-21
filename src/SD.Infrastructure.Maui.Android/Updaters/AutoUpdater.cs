using Acr.UserDialogs;
using Android.Content;
using Android.OS;
using Java.IO;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Uri = Android.Net.Uri;

namespace SD.Infrastructure.Maui.Android.Updaters
{
    /// <summary>
    /// Android自动更新器
    /// </summary>
    public static class AutoUpdater
    {
        //Public

        #region # 开启自动更新 —— static async Task Start(Context context, string appCast)
        /// <summary>
        /// 开启自动更新
        /// </summary>
        public static async Task Start(Context context, string appCast)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(appCast))
            {
                return;
            }
            if (!Ping(appCast))
            {
                return;
            }

            #endregion

            Release currentRelease = await GetCurrentRelease(appCast);
            Version currentVersion = new Version(VersionTracking.CurrentVersion);
            Version remoteVersion = new Version(currentRelease.Version);
            if (remoteVersion > currentVersion)
            {
                UserDialogs.Instance.ShowLoading("正在准备更新，请稍后...");
                await UpdateApplication(context, currentRelease.Url);
            }
        }
        #endregion


        //Private

        #region # Ping应用程序广播地址 —— static bool Ping(string appCast)
        /// <summary>
        /// Ping应用程序广播地址
        /// </summary>
        private static bool Ping(string appCast)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                Task<HttpResponseMessage> task = httpClient.GetAsync(appCast);
                if (!task.Wait(1000))
                {
                    return false;
                }

                HttpResponseMessage response = task.Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }

            return false;
        }
        #endregion

        #region # 获取当前发布 —— static async Task<Release> GetCurrentRelease(string appCast)
        /// <summary>
        /// 获取当前发布
        /// </summary>
        private static async Task<Release> GetCurrentRelease(string appCast)
        {
            using HttpClient httpClient = new HttpClient();
            string xml = await httpClient.GetStringAsync(appCast);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Release));
            XmlTextReader xmlTextReader = new XmlTextReader(new System.IO.StringReader(xml)) { XmlResolver = null };
            Release release = (Release)xmlSerializer.Deserialize(xmlTextReader);

            return release;
        }
        #endregion

        #region # 更新应用程序 —— static async Task UpdateApplication(Context context, string url)
        /// <summary>
        /// 更新应用程序
        /// </summary>
        private static async Task UpdateApplication(Context context, string url)
        {
            string apkName = Path.GetFileName(url);
            string localApkPath = Path.Combine(FileSystem.CacheDirectory, apkName);

            using HttpClient httpClient = new HttpClient();
            byte[] buffer = await httpClient.GetByteArrayAsync(url);
            SaveLocally(buffer, localApkPath);
            Install(context, localApkPath);
        }
        #endregion

        #region # 保存到本地 —— static void SaveLocally(byte[] buffer, string path)
        /// <summary>
        /// 保存到本地
        /// </summary>
        private static void SaveLocally(byte[] buffer, string path)
        {
            #region # 验证

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            #endregion

            Java.IO.File file = new Java.IO.File(path!);
            FileOutputStream stream = new FileOutputStream(file);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
        }
        #endregion

        #region # 执行安装 —— static void Install(Context context, string path)
        /// <summary>
        /// 执行安装
        /// </summary>
        private static void Install(Context context, string path)
        {
            Java.IO.File apkFile = new Java.IO.File(path);
            string apkMime = "application/vnd.android.package-archive";

            Intent intent = new Intent(Intent.ActionView);
            intent.AddFlags(ActivityFlags.NewTask);

            Uri fileUri;
            if (Build.VERSION.SdkInt > BuildVersionCodes.M)
            {
                fileUri = AndroidX.Core.Content.FileProvider.GetUriForFile(context, $"{AppInfo.PackageName}.fileProvider", apkFile);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            }
            else
            {
                fileUri = Uri.FromFile(apkFile);
            }

            intent.SetDataAndType(fileUri, apkMime);
            context.StartActivity(intent);

            UserDialogs.Instance.HideLoading();
            Process.KillProcess(Process.MyPid());
        }
        #endregion
    }
}
