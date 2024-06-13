using System.Windows;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 可改变尺寸Canvas
    /// </summary>
    public class ResizableCanvas : DraggableCanvas
    {
        #region # 字段及构造器

        /// <summary>
        /// 元素可改变尺寸依赖属性
        /// </summary>
        public static readonly DependencyProperty ResizableProperty;

        /// <summary>
        /// 元素改变尺寸路由事件
        /// </summary>
        public static readonly RoutedEvent ElementResizeEvent;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ResizableCanvas()
        {
            ResizableProperty = DependencyProperty.Register(nameof(Resizable), typeof(bool), typeof(ResizableCanvas), new PropertyMetadata(true));
            ElementResizeEvent = EventManager.RegisterRoutedEvent(nameof(ElementResize), RoutingStrategy.Direct, typeof(ElementResizeEventHandler), typeof(ResizableCanvas));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public ResizableCanvas()
        {
            base.MouseMove += this.OnMouseMove;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 元素可改变尺寸 —— bool Resizable
        /// <summary>
        /// 依赖属性 - 元素可改变尺寸
        /// </summary>
        public bool Resizable
        {
            get => (bool)this.GetValue(ResizableProperty);
            set => this.SetValue(ResizableProperty, value);
        }
        #endregion

        #region 附加属性 - 元素可改变尺寸 —— bool Resizable

        /// <summary>
        /// 获取元素可改变尺寸
        /// </summary>
        public static bool GetResizable(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(ResizableProperty);
        }

        /// <summary>
        /// 设置元素可改变尺寸
        /// </summary>
        public static void SetResizable(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(ResizableProperty, value);
        }

        #endregion

        #region 路由事件 - 元素改变尺寸 —— event ElementResizeEventHandler ElementResize
        /// <summary>
        /// 路由事件 - 元素改变尺寸
        /// </summary>
        public event ElementResizeEventHandler ElementResize
        {
            add { base.AddHandler(ElementResizeEvent, value); }
            remove { base.RemoveHandler(ElementResizeEvent, value); }
        }
        #endregion

        #endregion

        #region # 方法

        #region 鼠标移动事件 —— void OnMouseMove(object sender, MouseEventArgs eventArgs)
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs eventArgs)
        {
            ResizableCanvas canvas = (ResizableCanvas)sender;
            if (eventArgs.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown(Key.LeftCtrl) && canvas.SelectedVisual != null)
            {
                bool elementIsResizable = GetResizable(canvas.SelectedVisual);
                if (elementIsResizable)
                {
                    //设置光标
                    Mouse.OverrideCursor = Cursors.SizeNWSE;

                    //挂起路由事件
                    Point mousePosition = eventArgs.GetPosition(canvas);
                    Point retifiedMousePosition = canvas.MatrixTransform.Inverse!.Transform(mousePosition);
                    this.RaiseEvent(new ElementResizeEventArgs(ElementResizeEvent, canvas, retifiedMousePosition));
                }
            }
        }
        #endregion

        #endregion
    }


    /// <summary>
    /// 元素改变尺寸事件处理程序
    /// </summary>
    public delegate void ElementResizeEventHandler(ResizableCanvas canvas, ElementResizeEventArgs eventArgs);


    /// <summary>
    /// 元素改变尺寸事件参数
    /// </summary>
    public class ElementResizeEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 创建元素改变尺寸事件参数构造器
        /// </summary>
        public ElementResizeEventArgs(RoutedEvent routedEvent, object source, Point retifiedMousePosition)
            : base(routedEvent, source)
        {
            this.RetifiedMousePosition = retifiedMousePosition;
        }

        /// <summary>
        /// 矫正鼠标位置
        /// </summary>
        public Point RetifiedMousePosition { get; set; }
    }
}
