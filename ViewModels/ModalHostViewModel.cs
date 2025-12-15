using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MasterplanXP.ViewModels
{
    public partial class ModalHostViewModel : ObservableObject
    {
        public ModalHostViewModel(object contentViewModel, string title)
        {
            // The Content property holds the ViewModel for the module being hosted (e.g., MapViewModel)
            Content = contentViewModel;
            // The Title property holds the window title string
            Title = title;
        }

        // This is the property the XAML will bind to for the main content
        public object Content { get; }

        // This is the property the XAML will bind to for the window title
        public string Title { get; }
    }
}