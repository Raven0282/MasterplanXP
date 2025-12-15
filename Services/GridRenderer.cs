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
    /// Handles the drawing of the grid overlay onto the SkiaSharp canvas.
    /// This resolves the 'GridRenderer' not found error.
    /// </summary>
    public static class GridRenderer
    {
        private static readonly SKPaint GridPen = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.LightGray.WithAlpha(128), // 50% opacity gray
            StrokeWidth = 1.0f,
            IsAntialias = false
        };

        /// <summary>
        /// Draws the grid lines (currently only Square/Orthogonal).
        /// </summary>
        public static void Draw(SKCanvas canvas, Size bounds, double tileSize, GridType gridType, bool isIsometric)
        {
            // Only implements Square grid for now to enable compilation
            if (gridType != GridType.Square || isIsometric)
            {
                return;
            }

            float width = (float)bounds.Width;
            float height = (float)bounds.Height;
            float step = (float)tileSize;

            // Draw vertical lines
            for (float x = 0; x <= width; x += step)
            {
                canvas.DrawLine(x, 0, x, height, GridPen);
            }

            // Draw horizontal lines
            for (float y = 0; y <= height; y += step)
            {
                canvas.DrawLine(0, y, width, y, GridPen);
            }
        }
    }
}