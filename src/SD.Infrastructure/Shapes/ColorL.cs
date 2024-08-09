namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// RGBA颜色
    /// </summary>
    public readonly struct ColorL
    {
        /// <summary>
        /// 只读字段 - 空颜色
        /// </summary>
        public static readonly ColorL Empty = new ColorL(0, 0, 0, 0);

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

        /// <summary>
        /// 是否相等
        /// </summary>
        public override bool Equals(object instance)
        {
            if (instance is ColorL color)
            {
                return color == this;
            }

            return false;
        }

        /// <summary>
        /// 获取哈希码
        /// </summary>
        public override int GetHashCode()
        {
            return this.A.GetHashCode() ^ this.R.GetHashCode() ^ this.G.GetHashCode() ^ this.B.GetHashCode();
        }

        /// <summary>
        /// 比较颜色是否相等
        /// </summary>
        /// <param name="source">源颜色</param>
        /// <param name="target">目标颜色</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(ColorL source, ColorL target)
        {
            return source.A == target.A &&
                   source.R == target.R &&
                   source.G == target.G &&
                   source.B == target.B;
        }

        /// <summary>
        /// 比较颜色是否不等
        /// </summary>
        /// <param name="source">源颜色</param>
        /// <param name="target">目标颜色</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(ColorL source, ColorL target)
        {
            return !(source == target);
        }
    }
}
