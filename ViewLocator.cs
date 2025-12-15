using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MasterplanXP.ViewModels;
using MasterplanXP.Views; // Ensure you have this using statement
using System.Collections.Generic;
using System.Linq;


namespace MasterplanXP
{
    public class ViewLocator : IDataTemplate
    {

        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.OrdinalIgnoreCase);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            // Check for ObservableObject instead of only ViewModelBase
            return data is CommunityToolkit.Mvvm.ComponentModel.ObservableObject;
        }
    }
}
