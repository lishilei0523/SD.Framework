using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SD.Infrastructure.Avalonia.Models;

namespace SD.Infrastructure.Avalonia.Attachers
{
    /// <summary>
    /// Grid扩张器
    /// </summary>
    public class GridExpander
    {
        #region # 字段及构造器

        /// <summary>
        /// 原始状态属性
        /// </summary>
        private static readonly AttachedProperty<GridState> _OriginalStateProperty;

        /// <summary>
        /// 是否可扩张
        /// </summary>
        public static readonly AttachedProperty<bool> CanExpandProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GridExpander()
        {
            _OriginalStateProperty = AvaloniaProperty.RegisterAttached<GridExpander, Control, GridState>("OriginalState", null);
            CanExpandProperty = AvaloniaProperty.RegisterAttached<GridExpander, Control, bool>("CanExpand", false);

            //属性变化事件
            CanExpandProperty.Changed.AddClassHandler<Control>(OnCanExpandChanged);
        }

        #endregion

        #region # 属性

        #region 附加属性 - 原始状态 —— static GridState OriginalState
        /// <summary>
        /// 获取原始状态
        /// </summary>
        private static GridState GetOriginalState(Control element)
        {
            return element.GetValue(_OriginalStateProperty);
        }

        /// <summary>
        /// 设置原始状态
        /// </summary>
        private static void SetOriginalState(Control element, GridState value)
        {
            element.SetValue(_OriginalStateProperty, value);
        }
        #endregion

        #region 附加属性 - 是否可扩张 —— static bool CanExpand
        /// <summary>
        /// 设置是否可扩张
        /// </summary>
        public static bool GetCanExpand(Control element)
        {
            return element.GetValue(CanExpandProperty);
        }

        /// <summary>
        /// 获取是否可扩张
        /// </summary>
        public static void SetCanExpand(Control element, bool value)
        {
            element.SetValue(CanExpandProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 鼠标双击事件 —— static void OnDoubleTapped(object sender...
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        private static void OnDoubleTapped(object sender, RoutedEventArgs eventArgs)
        {
            if (sender is not Control element)
            {
                return;
            }

            Grid parentGrid = FindParentGrid(element);
            if (parentGrid == null)
            {
                return;
            }

            GridState state = GetOriginalState(element);
            if (state == null || !state.IsExpanded)
            {
                Expand(element, parentGrid);
            }
            else
            {
                Restore(element, parentGrid, state);
            }

            eventArgs.Handled = true;
        }
        #endregion

        #region 扩张 —— static void Expand(Control element, Grid parentGrid)
        /// <summary>
        /// 扩张
        /// </summary>
        private static void Expand(Control element, Grid parentGrid)
        {
            //保存当前控件状态
            GridState state = new GridState
            {
                Row = Grid.GetRow(element),
                Column = Grid.GetColumn(element),
                RowSpan = Grid.GetRowSpan(element),
                ColumnSpan = Grid.GetColumnSpan(element),
                ZIndex = element.ZIndex,
                Margin = element.Margin,
                IsExpanded = true
            };
            SetOriginalState(element, state);

            //隐藏其他兄弟
            foreach (Control child in parentGrid.Children)
            {
                if (child is not null && child != element)
                {
                    child.IsVisible = false;
                }
            }

            //展开当前控件
            Grid.SetRow(element, 0);
            Grid.SetColumn(element, 0);
            Grid.SetRowSpan(element, parentGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(element, parentGrid.ColumnDefinitions.Count);
            element.ZIndex = int.MaxValue;
            element.Margin = new Thickness(0);
        }
        #endregion

        #region 保存状态 —— static void Restore(Control element, Grid parentGrid...
        /// <summary>
        /// 保存状态
        /// </summary>
        private static void Restore(Control element, Grid parentGrid, GridState gridState)
        {
            //恢复当前控件
            Grid.SetRow(element, gridState.Row);
            Grid.SetColumn(element, gridState.Column);
            Grid.SetRowSpan(element, gridState.RowSpan);
            Grid.SetColumnSpan(element, gridState.ColumnSpan);
            element.ZIndex = gridState.ZIndex;
            element.Margin = gridState.Margin;

            //恢复其他兄弟
            foreach (Control child in parentGrid.Children)
            {
                if (child is not null && child != element)
                {
                    child.IsVisible = true;
                }
            }

            SetOriginalState(element, null);
        }
        #endregion

        #region 是否可扩张变化事件 —— static void OnCanExpandChanged(Control control...
        /// <summary>
        /// 是否可扩张变化事件
        /// </summary>
        private static void OnCanExpandChanged(Control control, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue is true)
            {
                control.DoubleTapped += OnDoubleTapped;
            }
            else
            {
                control.DoubleTapped -= OnDoubleTapped;
            }
        }
        #endregion

        #region 查找上级Grid元素 —— static Grid FindParentGrid(Control element)
        /// <summary>
        /// 查找上级Grid元素
        /// </summary>
        private static Grid FindParentGrid(Control element)
        {
            StyledElement parent = element?.Parent;
            while (parent != null)
            {
                if (parent is Grid grid)
                {
                    return grid;
                }
                parent = parent.Parent;
            }

            return null;
        }
        #endregion 

        #endregion
    }
}
