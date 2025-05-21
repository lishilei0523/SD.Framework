using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SD.Infrastructure.Avalonia.UserControls
{
    public partial class Loading : UserControl
    {
        public Loading()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
