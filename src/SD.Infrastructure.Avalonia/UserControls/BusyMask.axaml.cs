using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SD.Infrastructure.Avalonia.UserControls
{
    public partial class BusyMask : UserControl
    {
        public static readonly StyledProperty<bool> IsBusyProperty;

        public static readonly StyledProperty<string> BusyTextProperty;

        static BusyMask()
        {
            IsBusyProperty = AvaloniaProperty.Register<BusyMask, bool>(nameof(IsBusy), false);
            BusyTextProperty = AvaloniaProperty.Register<BusyMask, string>(nameof(BusyText), "Please wait...");
        }

        public BusyMask()
        {
            this.InitializeComponent();
        }

        public bool IsBusy
        {
            get => this.GetValue(IsBusyProperty);
            set => this.SetValue(IsBusyProperty, value);
        }

        public string BusyText
        {
            get => this.GetValue(BusyTextProperty);
            set => this.SetValue(BusyTextProperty, value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
