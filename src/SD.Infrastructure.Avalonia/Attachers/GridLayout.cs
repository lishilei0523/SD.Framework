using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using SD.Infrastructure.Avalonia.Models;

namespace SD.Infrastructure.Avalonia.Attachers
{
    /// <summary>
    /// Grid布局
    /// </summary>
    public class GridLayout
    {
        #region # 字段及构造器

        /// <summary>
        /// 布局配置属性
        /// </summary>
        public static readonly AttachedProperty<LayoutBase> ConfigProperty;

        /// <summary>
        /// 单元格属性
        /// </summary>
        public static readonly AttachedProperty<GridCell> CellProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static GridLayout()
        {
            ConfigProperty = AvaloniaProperty.RegisterAttached<GridLayout, Grid, LayoutBase>("Config", null);
            CellProperty = AvaloniaProperty.RegisterAttached<GridLayout, Control, GridCell>("Cell", null);

            //属性变化事件
            ConfigProperty.Changed.AddClassHandler<Grid>(OnConfigChanged);
            CellProperty.Changed.AddClassHandler<Control>(OnCellChanged);
        }

        #endregion

        #region # 属性

        #region 附加属性 - 布局配置 —— static LayoutBase Config
        /// <summary>
        /// 获取布局配置
        /// </summary>
        public static LayoutBase GetConfig(Grid grid)
        {
            return grid.GetValue(ConfigProperty);
        }

        /// <summary>
        /// 设置布局配置
        /// </summary>
        public static void SetConfig(Grid grid, LayoutBase value)
        {
            grid.SetValue(ConfigProperty, value);
        }
        #endregion

        #region 附加属性 - 单元格 —— static GridCell Cell
        /// <summary>
        /// 获取单元格
        /// </summary>
        public static GridCell GetCell(Control control)
        {
            return control.GetValue(CellProperty);
        }

        /// <summary>
        /// 设置单元格
        /// </summary>
        public static void SetCell(Control control, GridCell value)
        {
            control.SetValue(CellProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 布局配置变更事件 —— static void OnConfigChanged(Grid grid...
        /// <summary>
        /// 布局配置变更事件
        /// </summary>
        private static void OnConfigChanged(Grid grid, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            #region # 验证

            if (eventArgs.OldValue == eventArgs.NewValue)
            {
                return;
            }

            #endregion

            //重置所有子元素
            foreach (Control child in grid.Children)
            {
                child.IsVisible = true;
                Grid.SetRowSpan(child, 1);
                Grid.SetColumnSpan(child, 1);
            }

            //替换行列定义
            if (eventArgs.NewValue is LayoutBase config)
            {
                config.BuildDefinitions();

                //赋值
                grid.RowDefinitions.Clear();
                grid.ColumnDefinitions.Clear();
                foreach (RowDefinition rowDefinition in config.RowDefinitions)
                {
                    RowDefinition row = new RowDefinition(rowDefinition.Height);
                    grid.RowDefinitions.Add(row);
                }
                foreach (ColumnDefinition columnDefinition in config.ColumnDefinitions)
                {
                    ColumnDefinition column = new ColumnDefinition(columnDefinition.Width);
                    grid.ColumnDefinitions.Add(column);
                }
            }
        }
        #endregion

        #region 单元格变更事件 —— static void OnCellChanged(Control control...
        /// <summary>
        /// 单元格变更事件
        /// </summary>
        private static void OnCellChanged(Control control, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            //解除旧绑定
            if (eventArgs.OldValue is GridCell)
            {
                control.ClearValue(Grid.RowProperty);
                control.ClearValue(Grid.ColumnProperty);
                control.ClearValue(Grid.RowSpanProperty);
                control.ClearValue(Grid.ColumnSpanProperty);
                control.ClearValue(Visual.IsVisibleProperty);
            }

            //建立新绑定
            if (eventArgs.NewValue is GridCell cell)
            {
                control.Bind(Grid.RowProperty, new Binding(nameof(GridCell.Row))
                {
                    Source = cell,
                    Mode = BindingMode.OneWay
                });
                control.Bind(Grid.ColumnProperty, new Binding(nameof(GridCell.Column))
                {
                    Source = cell,
                    Mode = BindingMode.OneWay
                });
                control.Bind(Grid.RowSpanProperty, new Binding(nameof(GridCell.RowSpan))
                {
                    Source = cell,
                    Mode = BindingMode.OneWay
                });
                control.Bind(Grid.ColumnSpanProperty, new Binding(nameof(GridCell.ColumnSpan))
                {
                    Source = cell,
                    Mode = BindingMode.OneWay
                });
                control.Bind(Visual.IsVisibleProperty, new Binding(nameof(GridCell.IsVisible))
                {
                    Source = cell,
                    Mode = BindingMode.OneWay
                });
            }
        }
        #endregion

        #endregion
    }
}
