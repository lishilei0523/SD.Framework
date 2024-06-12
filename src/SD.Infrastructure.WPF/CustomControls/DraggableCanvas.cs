using System.Windows;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 可拖拽Canvas
    /// </summary>
    public class DraggableCanvas : ScalableCanvas
    {
        #region # 字段及构造器

        /// <summary>
        /// 元素可拖拽依赖属性
        /// </summary>
        public static readonly DependencyProperty DraggableProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DraggableCanvas()
        {
            DraggableProperty = DependencyProperty.Register(nameof(Draggable), typeof(bool), typeof(DraggableCanvas), new PropertyMetadata(true));
        }

        /// <summary>
        /// 拖拽位置
        /// </summary>
        private Vector _draggingPosition;

        /// <summary>
        /// 选中元素
        /// </summary>
        private UIElement _selectedVisual;

        /// <summary>
        /// 默认构造器
        /// </summary>
        public DraggableCanvas()
        {
            base.MouseDown += this.OnMouseDown;
            base.MouseMove += this.OnMouseMove;
            base.MouseUp += this.OnMouseUp;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 元素可拖拽 —— bool Draggable
        /// <summary>
        /// 依赖属性 - 元素可拖拽
        /// </summary>
        public bool Draggable
        {
            get => (bool)this.GetValue(DraggableProperty);
            set => this.SetValue(DraggableProperty, value);
        }
        #endregion

        #region 附加属性 - 元素可拖拽 —— bool DraggableCanvas.Draggable

        /// <summary>
        /// 获取元素可拖拽
        /// </summary>
        public static bool GetDraggable(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(DraggableProperty);
        }

        /// <summary>
        /// 设置元素可拖拽
        /// </summary>
        public static void SetDraggable(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(DraggableProperty, value);
        }

        #endregion

        #region 只读属性 - 拖拽位置 —— Vector DraggingPosition
        /// <summary>
        /// 只读属性 - 拖拽位置
        /// </summary>
        public Vector DraggingPosition
        {
            get => this._draggingPosition;
        }
        #endregion

        #region 只读属性 - 选中元素 —— UIElement SelectedVisual
        /// <summary>
        /// 只读属性 - 选中元素
        /// </summary>
        public UIElement SelectedVisual
        {
            get => this._selectedVisual;
        }
        #endregion

        #endregion

        #region # 方法

        #region 鼠标按下事件 —— void OnMouseDown(object sender, MouseButtonEventArgs eventArgs)
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        private void OnMouseDown(object sender, MouseButtonEventArgs eventArgs)
        {
            if (eventArgs.ChangedButton == MouseButton.Left)
            {
                UIElement element = (UIElement)eventArgs.Source;
                if (this.Children.Contains(element))
                {
                    Point mousePosition = eventArgs.GetPosition(this);
                    double elementX = double.IsNaN(GetLeft(element)) ? 0 : GetLeft(element);
                    double elementY = double.IsNaN(GetTop(element)) ? 0 : GetTop(element);
                    Point elementPosition = new Point(elementX, elementY);
                    this._selectedVisual = element;
                    this._draggingPosition = elementPosition - mousePosition;
                }
            }
        }
        #endregion

        #region 鼠标移动事件 —— void OnMouseMove(object sender, MouseEventArgs eventArgs)
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs eventArgs)
        {
            Point mousePosition = eventArgs.GetPosition(this);
            if (eventArgs.LeftButton == MouseButtonState.Pressed &&
                Keyboard.IsKeyUp(Key.LeftCtrl) &&
                Keyboard.IsKeyUp(Key.LeftAlt) &&
                Keyboard.IsKeyUp(Key.LeftShift) &&
                Keyboard.IsKeyUp(Key.RightCtrl) &&
                Keyboard.IsKeyUp(Key.RightAlt) &&
                Keyboard.IsKeyUp(Key.RightShift) &&
                Keyboard.IsKeyUp(Key.Space) &&
                this._selectedVisual != null)
            {
                bool elementIsDraggable = GetDraggable(this._selectedVisual);
                if (elementIsDraggable)
                {
                    //设置光标
                    Mouse.OverrideCursor = Cursors.Hand;

                    SetLeft(this._selectedVisual, mousePosition.X + this._draggingPosition.X);
                    SetTop(this._selectedVisual, mousePosition.Y + this._draggingPosition.Y);
                }
            }
        }
        #endregion

        #region 鼠标松开事件 —— void OnMouseUp(object sender, MouseButtonEventArgs eventArgs)
        /// <summary>
        /// 鼠标松开事件
        /// </summary>
        private void OnMouseUp(object sender, MouseButtonEventArgs eventArgs)
        {
            //设置光标
            Mouse.OverrideCursor = Cursors.Arrow;

            this._selectedVisual = null;
        }
        #endregion

        #endregion
    }
}
