using System.Runtime.InteropServices;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// RGBA颜色
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ColorL
    {
        /// <summary>
        /// 创建RGB颜色构造器
        /// </summary>
        /// <param name="r">R值</param>
        /// <param name="g">G值</param>
        /// <param name="b">B值</param>
        public ColorL(byte r, byte g, byte b)
            : this(r, g, b, byte.MaxValue)
        {

        }

        /// <summary>
        /// 创建RGBA颜色构造器
        /// </summary>
        /// <param name="r">R值</param>
        /// <param name="g">G值</param>
        /// <param name="b">B值</param>
        /// <param name="a">A值</param>
        public ColorL(byte r, byte g, byte b, byte a)
            : this()
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// R值
        /// </summary>
        public readonly byte R;

        /// <summary>
        /// G值
        /// </summary>
        public readonly byte G;

        /// <summary>
        /// B值
        /// </summary>
        public readonly byte B;

        /// <summary>
        /// A值
        /// </summary>
        public readonly byte A;

        /// <summary>
        /// 反转颜色
        /// </summary>
        /// <returns>相反颜色</returns>
        public ColorL Invert()
        {
            byte nr = (byte)(255 - this.R);
            byte ng = (byte)(255 - this.G);
            byte nb = (byte)(255 - this.B);

            return new ColorL(nr, ng, nb);
        }
    }
}
