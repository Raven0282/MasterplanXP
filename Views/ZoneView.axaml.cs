using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MasterplanXP.ViewModels;
using System;
using System.ComponentModel;

namespace MasterplanXP.Views
{
    public partial class ZoneView : UserControl
    {
        public ZoneView()
        {
            InitializeComponent();

            // Wait for the DataContext to be available
            this.DataContextChanged += ZoneView_DataContextChanged;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ZoneView_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is ZoneViewModel zoneVM)
            {
                // Initial configuration of the shape
                ConfigureShape(zoneVM.ShapeType);

                // Subscribe to changes in the ShapeType property
                zoneVM.PropertyChanged += ZoneVM_PropertyChanged;
            }
        }

        private void ZoneVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ZoneViewModel.ShapeType) && DataContext is ZoneViewModel zoneVM)
            {
                ConfigureShape(zoneVM.ShapeType);
            }
        }

        /// <summary>
        /// Configures the visual appearance of the zone based on its ShapeType property.
        /// </summary>
        private void ConfigureShape(string shapeType)
        {
            if (this.FindControl<Border>("ShapeBorder") is not Border shapeBorder) return;

            // Reset common properties
            shapeBorder.CornerRadius = new CornerRadius(0);
            shapeBorder.Classes.Clear();

            switch (shapeType.ToLowerInvariant())
            {
                case "ellipse":
                    // To make a Border appear as an ellipse, set CornerRadius to half the control's dimension.
                    // Since the size is dynamic, we apply a style class instead, but for simplicity:
                    shapeBorder.CornerRadius = new CornerRadius(Math.Max(shapeBorder.Bounds.Width, shapeBorder.Bounds.Height));
                    shapeBorder.Classes.Add("zone-ellipse");
                    break;
                case "rectangle":
                default:
                    shapeBorder.CornerRadius = new CornerRadius(0);
                    shapeBorder.Classes.Add("zone-rectangle");
                    break;
                    // Note: Complex shapes like "Polygon" would require swapping the Border 
                    // for a dedicated Polygon control via a DataTemplateSelector or similar method.
            }
        }
    }
}
