using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ShSoft.Framework2016.Common.PoweredByLee
{
    /// <summary>
    /// 流扩展方法
    /// </summary>
    public static class StreamExtension
    {
        #region # 计算流的MD5值扩展方法 —— static string ToMD5(this Stream stream)
        /// <summary>
        /// 计算流的MD5值扩展方法
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>MD5值</returns>
        public static string ToMD5(this Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                foreach (byte byt in buffer)
                {
                    sb.Append(byt.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        #endregion
    }
}
