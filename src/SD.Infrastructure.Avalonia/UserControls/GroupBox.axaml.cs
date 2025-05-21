using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SD.Infrastructure.Avalonia.UserControls
{
    public partial class GroupBox : UserControl
    {
        public static readonly StyledProperty<object> HeaderProperty;

        static GroupBox()
        {
            HeaderProperty = AvaloniaProperty.Register<GroupBox, object>(nameof(Header), "Header");
        }

        public GroupBox()
        {
            this.InitializeComponent();
        }

        public object Header
        {
            get => this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
