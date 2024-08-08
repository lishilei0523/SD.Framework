using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Visual2Ds
{
    /// <summary>
    /// 形状呈现器
    /// </summary>
    public class ShapePresenter : Shape
    {
        #region # 字段及构造器

        /// <summary>
        /// 内容依赖属性
        /// </summary>
        public static readonly DependencyProperty ContentProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ShapePresenter()
        {
            ContentProperty = DependencyProperty.Register(nameof(Content), typeof(Shape), typeof(ShapePresenter), new PropertyMetadata(null, OnContentChanged));
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 内容 —— Shape Content
        /// <summary>
        /// 依赖属性 - 内容
        /// </summary>
        public Shape Content
        {
            get => (Shape)this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }
        #endregion

        #region 只读属性 - 几何对象 —— override Geometry DefiningGeometry
        /// <summary>
        /// 只读属性 - 几何对象
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (this.Content != null)
                {
                    Type type = typeof(Shape);
                    PropertyInfo property = type.GetProperty(nameof(this.DefiningGeometry), BindingFlags.Instance | BindingFlags.NonPublic);
                    Geometry geometry = (Geometry)property!.GetValue(this.Content, null);
                    return geometry;
                }

                return null;
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 内容改变事件 —— static void OnContentChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 内容改变事件
        /// </summary>
        private static void OnContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ShapePresenter shapePresenter = (ShapePresenter)dependencyObject;
            if (eventArgs.NewValue is Shape shape)
            {
                Type shapePresenterType = typeof(ShapePresenter);
                Type shapeType = typeof(Shape);
                PropertyInfo[] shapePresenterProperties = shapePresenterType.GetProperties();
                PropertyInfo[] shapeProperties = shapeType.GetProperties();
                foreach (PropertyInfo shapePresenterProperty in shapePresenterProperties.Where(x => x.GetSetMethod() != null))
                {
                    PropertyInfo shapeProperty = shapeProperties.SingleOrDefault(x => x.Name == shapePresenterProperty.Name);
                    if (shapeProperty != null)
                    {
                        object shapePropertyValue = shapeProperty.GetValue(shape, null);
                        shapePresenterProperty.SetValue(shapePresenter, shapePropertyValue, null);
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}
