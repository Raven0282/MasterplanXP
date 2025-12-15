using Avalonia;

namespace MasterplanXP.Models
{
    public class Token
    {
        public Avalonia.Point LogicalPosition { get; set; } = new Avalonia.Point();
        public int Elevation { get; set; } = 0;
        // Add other properties required by TokenRenderer.Draw if needed
    }
}