using System;
using System.IO;
using System.Text;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 文件操作帮助类
    /// </summary>
    public static class FileAssistant
    {
        #region 01.读取文件方法 —— static string ReadFile(string path)
        /// <summary>
        /// 读取文件方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>内容字符串</returns>
        /// <exception cref="ArgumentNullException">路径为空</exception>
        public static string ReadFile(string path)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), @"路径不可为空！");
            }

            #endregion

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(path, Encoding.UTF8);
                string content = reader.ReadToEnd();
                return content;
            }
            finally
            {
                reader?.Dispose();
            }
        }
        #endregion

        #region 02.写入文件方法 —— static void WriteFile(string path, string content)
        /// <summary>
        /// 写入文件方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <param name="append">是否附加</param>
        /// <exception cref="ArgumentNullException">路径为空</exception>
        public static void WriteFile(string path, string content, bool append = false)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "路径不可为空！");
            }

            #endregion

            FileInfo file = new FileInfo(path);
            StreamWriter writer = null;

            if (file.Exists && !append)
            {
                file.Delete();
            }
            try
            {
                //获取文件目录并判断是否存在
                string directory = Path.GetDirectoryName(path);

                if (string.IsNullOrEmpty(directory))
                {
                    throw new ArgumentNullException(nameof(path), "目录不可为空！");
                }
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                writer = append ? file.AppendText() : new StreamWriter(path, false, Encoding.UTF8);
                writer.Write(content);
            }
            finally
            {
                writer?.Dispose();
            }
        }
        #endregion

        #region 03.复制文件夹方法 —— static void CopyFolder(string sourcePath, string targetPath)
        /// <summary>
        /// 复制文件夹方法
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        public static void CopyFolder(string sourcePath, string targetPath)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentNullException(nameof(sourcePath), "源路径不可为空！");
            }

            if (string.IsNullOrWhiteSpace(targetPath))
            {
                throw new ArgumentNullException(nameof(targetPath), "目标路径不可为空！");
            }

            #endregion

            //01.创建目标文件夹
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            //02.拷贝文件
            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);
            FileInfo[] fileArray = sourceDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                file.CopyTo($"{targetPath}\\{file.Name}", true);
            }

            //03.递归循环子文件夹
            DirectoryInfo[] subDirArray = sourceDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                CopyFolder(subDir.FullName, $"{targetPath}//{subDir.Name}");
            }
        }
        #endregion
    }
}
