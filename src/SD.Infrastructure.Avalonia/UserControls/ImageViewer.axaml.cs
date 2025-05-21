using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using SD.Infrastructure.Avalonia.Extensions;

namespace SD.Infrastructure.Avalonia.UserControls
{
    /// <summary>
    /// 图像查看控件
    /// </summary>
    public partial class ImageViewer : UserControl
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认缩放因子
        /// </summary>
        public const float ScaleFactor = 1.1F;

        /// <summary>
        /// 图像源依赖属性
        /// </summary>
        public static readonly StyledProperty<IImage> ImageSourceProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ImageViewer()
        {
            //注册依赖属性
            ImageSourceProperty = AvaloniaProperty.Register<ImageViewer, IImage>(nameof(ImageSource));
        }

        /// <summary>
        /// 顶点
        /// </summary>
        private Point _vertex;

        /// <summary>
        /// 是否正在移动
        /// </summary>
        private bool _moving;

        /// <summary>
        /// 构造器
        /// </summary>
        public ImageViewer()
        {
            this.InitializeComponent();
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 图像源 —— ImageSource ImageSource
        /// <summary>
        /// 依赖属性 - 图像源
        /// </summary>
        public IImage ImageSource
        {
            get => (IImage)base.GetValue(ImageSourceProperty);
            set => base.SetValue(ImageSourceProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region ViewBox鼠标点击事件 —— void OnViewBoxMouseDown(object sender...
        /// <summary>
        /// ViewBox鼠标点击事件
        /// </summary>
        private void OnViewBoxMouseDown(object sender, PointerPressedEventArgs eventArgs)
        {
            PointerPoint pointerPoint = eventArgs.GetCurrentPoint((Visual)sender);
            if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                this.Viewbox.Focus();
                this._vertex = eventArgs.GetPosition(this.Frame);
                this._moving = true;
            }
        }
        #endregion

        #region ViewBox鼠标移动事件 —— void OnViewBoxMouseMove(object sender...
        /// <summary>
        /// ViewBox鼠标移动事件
        /// </summary>
        private void OnViewBoxMouseMove(object sender, PointerEventArgs eventArgs)
        {
            #region # 验证

            if (!this._moving)
            {
                return;
            }

            #endregion

            PointerPoint pointerPoint = eventArgs.GetCurrentPoint((Visual)sender);
            if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                //设置光标
                this.Cursor = new Cursor(StandardCursorType.Hand);

                Point position = eventArgs.GetPosition(this.Frame);
                Matrix matrix = this.Viewbox.RenderTransform!.Value;

                //计算偏移量
                double offsetX = matrix.M31 + position.X - this._vertex.X;
                double offsetY = matrix.M32 + position.Y - this._vertex.Y;
                Matrix newMatrix = new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M21, matrix.M22, matrix.M23, offsetX, offsetY, matrix.M33);

                //给鼠标起点和图像临时位置重新赋值
                this._vertex = position;
                this.Viewbox.RenderTransform = new MatrixTransform(newMatrix);
            }
        }
        #endregion 

        #region ViewBox鼠标滚轮事件 —— void OnViewBoxMouseWheel(object sender...
        /// <summary>
        /// ViewBox鼠标滚轮事件
        /// </summary>
        private void OnViewBoxMouseWheel(object sender, PointerWheelEventArgs eventArgs)
        {
            Point position = eventArgs.GetPosition(this.Viewbox);
            MatrixTransform matrixTransform = (MatrixTransform)this.Viewbox.RenderTransform;
            Matrix matrix = matrixTransform!.Matrix;
            if (eventArgs.Delta.Y > 0)
            {
                matrix = matrix.ScaleAt(ScaleFactor, ScaleFactor, position.X, position.Y);
            }
            else
            {
                matrix = matrix.ScaleAt(1 / ScaleFactor, 1 / ScaleFactor, position.X, position.Y);
            }

            this.Viewbox.RenderTransform = new MatrixTransform(matrix);
        }
        #endregion

        #region ViewBox鼠标松开事件 —— void OnViewBoxMouseUp(object sender...
        /// <summary>
        /// ViewBox鼠标松开事件
        /// </summary>
        private void OnViewBoxMouseUp(object sender, PointerReleasedEventArgs eventArgs)
        {
            if (eventArgs.InitialPressMouseButton == MouseButton.Middle)
            {
                //设置光标
                this.Cursor = new Cursor(StandardCursorType.Arrow);

                this._moving = false;
            }
        }
        #endregion

        #endregion
    }
}
