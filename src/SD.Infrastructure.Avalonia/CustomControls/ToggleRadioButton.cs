using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;

namespace SD.Infrastructure.Avalonia.CustomControls
{
    /// <summary>
    /// Toggle形式RadioButton
    /// </summary>
    public class ToggleRadioButton : RadioButton
    {
        /// <summary>
        /// 样式键覆盖
        /// </summary>
        protected override Type StyleKeyOverride
        {
            get => typeof(ToggleButton);
        }
    }
}
