using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SD.Infrastructure.WPF.UserControls
{
    /// <summary>
    /// 图片查看控件
    /// </summary>
    public partial class ImageViewer
    {
        #region # 字段及构造器

        /// <summary>
        /// 缩放系数
        /// </summary>
        private const float ScaleFactor = 1.1F;

        /// <summary>
        /// 图片源依赖属性
        /// </summary>
        public static DependencyProperty ImageSourceProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ImageViewer()
        {
            //注册依赖属性
            ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(ImageViewer), new PropertyMetadata(null));
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

        #region 依赖属性 - 图片源 —— ImageSource ImageSource
        /// <summary>
        /// 依赖属性 - 图片源
        /// </summary>
        public ImageSource ImageSource
        {
            get => (ImageSource)base.GetValue(ImageSourceProperty);
            set => base.SetValue(ImageSourceProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region ViewBox鼠标点击事件 —— void OnViewBoxMouseDown(object sender...
        /// <summary>
        /// ViewBox鼠标点击事件
        /// </summary>
        private void OnViewBoxMouseDown(object sender, MouseEventArgs eventArgs)
        {
            #region # 验证

            if (this.Viewbox.IsMouseCaptured)
            {
                return;
            }

            #endregion

            if (eventArgs.MiddleButton == MouseButtonState.Pressed)
            {
                this.Viewbox.CaptureMouse();
                this._vertex = eventArgs.MouseDevice.GetPosition(this.Frame);
                this._moving = true;
            }
        }
        #endregion

        #region ViewBox鼠标移动事件 —— void OnViewBoxMouseMove(object sender...
        /// <summary>
        /// ViewBox鼠标移动事件
        /// </summary>
        private void OnViewBoxMouseMove(object sender, MouseEventArgs eventArgs)
        {
            #region # 验证

            if (!this.Viewbox.IsMouseCaptured || !this._moving)
            {
                return;
            }

            #endregion

            //设置光标
            Mouse.OverrideCursor = Cursors.Hand;

            Point position = eventArgs.MouseDevice.GetPosition(this.Frame);
            Matrix matrix = this.Viewbox.RenderTransform.Value;

            //计算偏移量 
            matrix.OffsetX = matrix.OffsetX + position.X - this._vertex.X;
            matrix.OffsetY = matrix.OffsetY + position.Y - this._vertex.Y;

            //给鼠标起点和图片临时位置重新赋值
            this._vertex = position;
            this.Viewbox.RenderTransform = new MatrixTransform(matrix);
        }
        #endregion 

        #region ViewBox鼠标滚轮事件 —— void OnViewBoxMouseWheel(object sender...
        /// <summary>
        /// ViewBox鼠标滚轮事件
        /// </summary>
        private void OnViewBoxMouseWheel(object sender, MouseWheelEventArgs eventArgs)
        {
            Point position = eventArgs.GetPosition(this.Viewbox);
            MatrixTransform matrixTransform = (MatrixTransform)this.Viewbox.RenderTransform;
            Matrix matrix = matrixTransform.Matrix;
            if (eventArgs.Delta > 0)
            {
                matrix.ScaleAtPrepend(ScaleFactor, ScaleFactor, position.X, position.Y);
            }
            else
            {
                matrix.ScaleAtPrepend(1 / ScaleFactor, 1 / ScaleFactor, position.X, position.Y);
            }

            this.Viewbox.RenderTransform = new MatrixTransform(matrix);
        }
        #endregion

        #region ViewBox鼠标松开事件 —— void OnViewBoxMouseUp(object sender...
        /// <summary>
        /// ViewBox鼠标松开事件
        /// </summary>
        private void OnViewBoxMouseUp(object sender, MouseEventArgs eventArgs)
        {
            if (eventArgs.MiddleButton == MouseButtonState.Released)
            {
                //设置光标
                Mouse.OverrideCursor = Cursors.Arrow;

                this.Viewbox.ReleaseMouseCapture();
                this._moving = false;
            }
        }
        #endregion

        #endregion
    }
}
