using Avalonia.Controls;
using Avalonia.Input;
using MasterplanXP.ViewModels;
using System.Reflection;

namespace MasterplanXP.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //MenuImage.AddHandler(PointerPressedEvent, new PointerEventHandler(MyPointerPressedHandler), true

        public void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
        {
            if (e.ClickCount != 2)
                return;

            (DataContext as MainWindowViewModel)?.SideMenuResizeCommand.Execute(null);
            
        }


        public void InputElementOnPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (e.ClickCount != 2)
                return;

            (DataContext as MainWindowViewModel)?.SideMenuResizeCommand?.Execute(null);
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            return;
        }

        private void Image_PointerPressed_1(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
        }
    }
}
