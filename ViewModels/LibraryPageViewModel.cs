using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MasterplanXP.ViewModels
{
    public partial class LibraryPageViewModel : ViewModelBase
    {

        [ObservableProperty]
        private ObservableCollection<string> _libraryNames = new()
        {        
            
            "Monster Vault",
            "Monster Manual",
            "Awesome Book",
            "Better Book"
            
        };

     


        public string Test { get; set; } = "Library";

    }
}
