using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SD.Infrastructure.WPF.UserControls
{
    /// <summary>
    /// 图像对比查看控件
    /// </summary>
    public partial class ImageCompareViewer
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认缩放因子
        /// </summary>
        public const float ScaleFactor = 1.1F;

        /// <summary>
        /// 图像源1依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageSource1Property;

        /// <summary>
        /// 图像源1透明度依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageSource1OpacityProperty;

        /// <summary>
        /// 图像源2依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageSource2Property;

        /// <summary>
        /// 图像源2透明度依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageSource2OpacityProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ImageCompareViewer()
        {
            //注册依赖属性
            ImageSource1Property = DependencyProperty.Register(nameof(ImageSource1), typeof(ImageSource), typeof(ImageCompareViewer), new PropertyMetadata(null));
            ImageSource1OpacityProperty = DependencyProperty.Register(nameof(ImageSource1Opacity), typeof(double), typeof(ImageCompareViewer), new PropertyMetadata(0.5d));
            ImageSource2Property = DependencyProperty.Register(nameof(ImageSource2), typeof(ImageSource), typeof(ImageCompareViewer), new PropertyMetadata(null));
            ImageSource2OpacityProperty = DependencyProperty.Register(nameof(ImageSource2Opacity), typeof(double), typeof(ImageCompareViewer), new PropertyMetadata(0.5d));
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
        public ImageCompareViewer()
        {
            this.InitializeComponent();
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 图像源1 —— ImageSource ImageSource1
        /// <summary>
        /// 依赖属性 - 图像源1
        /// </summary>
        public ImageSource ImageSource1
        {
            get => (ImageSource)base.GetValue(ImageSource1Property);
            set => base.SetValue(ImageSource1Property, value);
        }
        #endregion

        #region 依赖属性 - 图像源1透明度 —— double ImageSource1Opacity
        /// <summary>
        /// 依赖属性 - 图像源1透明度
        /// </summary>
        public double ImageSource1Opacity
        {
            get => (double)base.GetValue(ImageSource1OpacityProperty);
            set => base.SetValue(ImageSource1OpacityProperty, value);
        }
        #endregion

        #region 依赖属性 - 图像源2 —— ImageSource ImageSource2
        /// <summary>
        /// 依赖属性 - 图像源
        /// </summary>
        public ImageSource ImageSource2
        {
            get => (ImageSource)base.GetValue(ImageSource2Property);
            set => base.SetValue(ImageSource2Property, value);
        }
        #endregion

        #region 依赖属性 - 图像源2透明度 —— double ImageSource2Opacity
        /// <summary>
        /// 依赖属性 - 图像源2透明度
        /// </summary>
        public double ImageSource2Opacity
        {
            get => (double)base.GetValue(ImageSource2OpacityProperty);
            set => base.SetValue(ImageSource2OpacityProperty, value);
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

            if (eventArgs.MiddleButton == MouseButtonState.Pressed)
            {
                //设置光标
                Mouse.OverrideCursor = Cursors.Hand;

                Point position = eventArgs.MouseDevice.GetPosition(this.Frame);
                Matrix matrix = this.Viewbox.RenderTransform.Value;

                //计算偏移量
                matrix.OffsetX = matrix.OffsetX + position.X - this._vertex.X;
                matrix.OffsetY = matrix.OffsetY + position.Y - this._vertex.Y;

                //给鼠标起点和图像临时位置重新赋值
                this._vertex = position;
                this.Viewbox.RenderTransform = new MatrixTransform(matrix);
            }
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
