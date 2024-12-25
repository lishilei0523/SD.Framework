using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SD.Infrastructure.WPF.Animations
{
    /// <summary>
    /// 画刷动画
    /// </summary>
    public class BrushAnimation : AnimationTimeline
    {
        #region # 字段及构造器

        /// <summary>
        /// 源画刷依赖属性
        /// </summary>
        public static readonly DependencyProperty FromProperty;

        /// <summary>
        /// 目标画刷依赖属性
        /// </summary>
        public static readonly DependencyProperty ToProperty;

        /// <summary>
        /// 缓动函数依赖属性
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static BrushAnimation()
        {
            FromProperty = DependencyProperty.Register(nameof(From), typeof(Brush), typeof(BrushAnimation), new PropertyMetadata(null));
            ToProperty = DependencyProperty.Register(nameof(To), typeof(Brush), typeof(BrushAnimation), new PropertyMetadata(null));
            EasingFunctionProperty = DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(BrushAnimation), new PropertyMetadata(null));
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 源画刷 —— Brush From
        /// <summary>
        /// 依赖属性 - 源画刷
        /// </summary>
        public Brush From
        {
            get => (Brush)this.GetValue(FromProperty);
            set => this.SetValue(FromProperty, value);
        }
        #endregion

        #region 依赖属性 - 目标画刷 —— Brush To
        /// <summary>
        /// 依赖属性 - 目标画刷
        /// </summary>
        public Brush To
        {
            get => (Brush)this.GetValue(ToProperty);
            set => this.SetValue(ToProperty, value);
        }
        #endregion

        #region 依赖属性 - 缓动函数 —— IEasingFunction EasingFunction
        /// <summary>
        /// 依赖属性 - 缓动函数
        /// </summary>
        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)this.GetValue(EasingFunctionProperty);
            set => this.SetValue(EasingFunctionProperty, value);
        }
        #endregion

        #region 只读属性 - 目标属性类型 —— override Type TargetPropertyType
        /// <summary>
        /// 只读属性 - 目标属性类型
        /// </summary>
        public override Type TargetPropertyType
        {
            get => typeof(Brush);
        }
        #endregion

        #endregion

        #region # 方法

        #region 获取当前画刷值 —— override object GetCurrentValue(object defaultOriginValue...
        /// <summary>
        /// 获取当前画刷值
        /// </summary>
        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            #region # 验证

            if (!animationClock.CurrentProgress.HasValue)
            {
                return Brushes.Transparent;
            }

            #endregion

            //源/目标画刷设定
            Brush sourceBrush = this.From ?? defaultOriginValue as Brush;
            Brush targetBrush = this.To ?? defaultDestinationValue as Brush;

            //判断播放进度
            double progress = animationClock.CurrentProgress.Value;
            if (progress.Equals(0))
            {
                return sourceBrush!;
            }
            if (progress.Equals(1))
            {
                return targetBrush!;
            }

            //适用缓动函数
            if (this.EasingFunction != null)
            {
                progress = this.EasingFunction.Ease(progress);
            }

            Border border = new Border()
            {
                Width = 1,
                Height = 1,
                Background = sourceBrush,
                Child = new Border()
                {
                    Background = targetBrush,
                    Opacity = progress
                }
            };
            VisualBrush visualBrush = new VisualBrush(border);

            return visualBrush;
        }
        #endregion

        #region 创建可冻结对象 —— override Freezable CreateInstanceCore()
        /// <summary>
        /// 创建可冻结对象
        /// </summary>
        protected override Freezable CreateInstanceCore()
        {
            return new BrushAnimation();
        }
        #endregion 

        #endregion
    }
}
