namespace SD.Infrastructure.WPF.Enums
{
    /// <summary>
    /// 拖拽方向
    /// </summary>
    public enum DragDirection
    {
        /// <summary>
        /// 上左
        /// </summary>
        TopLeft = 1,

        /// <summary>
        /// 上中
        /// </summary>
        TopCenter = 2,

        /// <summary>
        /// 上右
        /// </summary>
        TopRight = 4,

        /// <summary>
        /// 中左
        /// </summary>
        MiddleLeft = 16,

        /// <summary>
        /// 中中
        /// </summary>
        MiddleCenter = 32,

        /// <summary>
        /// 中右
        /// </summary>
        MiddleRight = 64,

        /// <summary>
        /// 下左
        /// </summary>
        BottomLeft = 256,

        /// <summary>
        /// 下中
        /// </summary>
        BottomCenter = 512,

        /// <summary>
        /// 下右
        /// </summary>
        BottomRight = 1024
    }
}
