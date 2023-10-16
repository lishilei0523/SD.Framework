using SD.Infrastructure.WPF.Models;
using System;
using System.Windows;

namespace SD.Infrastructure.WPF.Draggable.Tests
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class Index : Window
    {
        public Index()
        {
            this.InitializeComponent();
        }

        private void DraggableControlBase_OnDragChanging(object sender, DraggedEventArgs eventArgs)
        {
            Console.WriteLine(sender);
            Console.WriteLine(eventArgs);
        }

        private void DraggableControlBase_OnDragCompleted(object sender, DraggedEventArgs eventArgs)
        {
            Console.WriteLine(sender);
            Console.WriteLine(eventArgs);
        }
    }
}
