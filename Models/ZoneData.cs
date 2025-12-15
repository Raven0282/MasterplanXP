using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MasterplanXP.Models
{
    /// <summary>
    /// Represents the data model for a map zone (overlay). This is a POCO used for serialization.
    /// </summary>
    /// <remarks>
    /// Zones use a logical coordinate set (X, Y, Width, Height) and support elevation.
    /// </remarks>
    public class ZoneData
    {
        /// <summary>
        /// A unique identifier for the zone.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The display name of the zone (e.g., "Area of Effect," "Stairs," "Balcony").
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Details displayed on hover (e.g., "Grants +1 AC," "Difficult Terrain").
        /// </summary>
        public string Details { get; set; } = string.Empty;

        // --- Position and Size (Logical Grid) ---

        /// <summary>
        /// The logical starting X-coordinate (top-left) on the map grid.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// The logical starting Y-coordinate (top-left) on the map grid.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The width of the zone in logical grid units.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The height of the zone in logical grid units.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// The logical elevation of the zone's base (0 for ground level).
        /// </summary>
        public int Elevation { get; set; } = 0;


        // --- Visual Properties ---

        /// <summary>
        /// The color representation (e.g., Hex code) of the zone overlay.
        /// </summary>
        public required string ColorHex { get; set; }

        /// <summary>
        /// The opacity of the zone overlay (0.0 to 1.0).
        /// </summary>
        public double Opacity { get; set; } = 0.5;

        /// <summary>
        /// The shape of the zone ("Rectangle", "Ellipse", "Polygon").
        /// </summary>
        public string ShapeType { get; set; } = "Rectangle";

        // Future use for complex shapes
        public List<Point> PolygonPoints { get; set; } = new List<Point>();
    }

    /// <summary>
    /// Simple struct to hold X/Y coordinates for polygon definition.
    /// </summary>
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}