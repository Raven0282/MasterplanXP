using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using MasterplanXP.Models;
using SkiaSharp;
using System.Diagnostics;

namespace MasterplanXP.Services
{
    /// <summary>
    /// Handles the drawing of individual tokens onto the SkiaSharp canvas.
    /// This resolves the 'TokenRenderer' not found error.
    /// </summary>
    public static class TokenRenderer
    {
        private static readonly SKPaint TokenPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Red,
            IsAntialias = true
        };

        /// <summary>
        /// Draws a representation of the token.
        /// Note: This logic is likely redundant with the TokenView XAML control, 
        /// but is required to make MapCanvas.cs compile.
        /// </summary>
        public static void Draw(SKCanvas canvas, Token token, double tileSize, bool isIsometric)
        {
            float size = (float)tileSize / 2f;

            // Calculate center based on logical position
            float centerX = (float)token.LogicalPosition.X * (float)tileSize + size;
            float centerY = (float)token.LogicalPosition.Y * (float)tileSize + size;

            // Draw a simple red circle as a placeholder
            canvas.DrawCircle(centerX, centerY, size / 2f, TokenPaint);
        }
    }
}