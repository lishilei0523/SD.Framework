using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace ShSoft.Framework2015.Common.PoweredByLee
{
    /// <summary>
    /// 验证码处理类
    /// </summary>
    public class ValidCode
    {
        #region 00.访问器 —— static ValidCode Current
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private ValidCode() { }

        /// <summary>
        /// 访问器
        /// </summary>
        public static ValidCode Current
        {
            get
            {
                //从线程缓存中取，如果没有new一个，然后再存入线程缓存
                ValidCode vCode = CallContext.GetData(typeof(ValidCode).Name) as ValidCode;
                if (vCode == null)
                {
                    vCode = new ValidCode();
                    CallContext.SetData(typeof(ValidCode).Name, vCode);
                }
                return vCode;
            }
        }
        #endregion

        #region 01.验证码的最大长度常量 —— int MaxLength
        /// <summary>
        /// 验证码的最大长度属性
        /// </summary>
        public const int MaxLength = 10;
        #endregion

        #region 02.验证码的最小长度常量 —— int MinLength
        /// <summary>
        /// 验证码的最小长度属性
        /// </summary>
        public const int MinLength = 1;
        #endregion

        #region 03.创建验证码字符串方法 —— string CreateValidCode(int length)
        /// <summary>
        /// 创建验证码字符串方法
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>验证码字符串</returns>
        public string CreateValidCode(int length)
        {
            #region # 验证参数

            if (length > MaxLength)
            {
                throw new ArgumentOutOfRangeException("length", "验证码长度不可超过10！");
            }

            if (length < MinLength)
            {
                throw new ArgumentOutOfRangeException("length", "验证码长度不小于1！");
            }

            #endregion

            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = string.Empty;
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
        #endregion

        #region 04.创建验证码图片方法（直接输出到浏览器） —— void CreateValidCodeImage(string validCode, HttpContext context)
        /// <summary>
        /// 创建验证码图片（直接输出到浏览器）
        /// </summary>
        /// <param name="validCode">验证码字符串</param>
        /// <param name="context">HttpContext上下文对象</param>
        public void CreateValidCodeImage(string validCode, HttpContext context)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validCode.Length * 12.0), 22);
            Graphics graphic = Graphics.FromImage(image);
            try
            {
                //绘制验证码
                MemoryStream stream = this.DrawValidCode(validCode, graphic, image);

                //输出图片流
                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(stream.ToArray());
            }
            finally
            {
                graphic.Dispose();
                image.Dispose();
            }
        }
        #endregion

        #region 05.创建验证码图片方法（返回字节数组） —— byte[] CreateValidCodeImage(string validCode)
        /// <summary>
        /// 创建验证码图片方法（返回字节数组）
        /// </summary>
        /// <param name="validCode">验证码字符串</param>
        /// <returns>验证码图片序列化后的字节数组</returns>
        public byte[] CreateValidCodeImage(string validCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validCode.Length * 12.0), 22);
            Graphics graphic = Graphics.FromImage(image);
            try
            {
                //绘制验证码
                MemoryStream stream = this.DrawValidCode(validCode, graphic, image);

                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                graphic.Dispose();
                image.Dispose();
            }
        }
        #endregion

        private MemoryStream DrawValidCode(string validCode, Graphics graphic, Bitmap image)
        {
            //生成随机生成器
            Random random = new Random();

            //清空图片背景色
            graphic.Clear(Color.White);

            //画图片的干扰线
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                graphic.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                Color.Blue, Color.DarkRed, 1.2f, true);
            graphic.DrawString(validCode, font, brush, 3, 2);
            //画图片的前景干扰点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //画图片的边框线
            graphic.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            //保存图片数据
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);

            return stream;
        }
    }
}
