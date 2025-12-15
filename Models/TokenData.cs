using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MasterplanXP.Models
{
    /// <summary>
    /// Represents the data model for a map token. This is a Plain Old C# Object (POCO) 
    /// used for serialization (saving/exporting).
    /// </summary>
    /// <remarks>
    /// This structure holds logical grid/map coordinates (not screen pixels).
    /// </remarks>
    public class TokenData
    {
        // --- Core Identifier and Naming ---

        /// <summary>
        /// A unique identifier for the token, useful for multi-user synchronization.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The display name of the token.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Filterable specifics/details displayed on hover (e.g., "HP: 50, Status: Poisoned").
        /// </summary>
        public string Details { get; set; } = string.Empty;


        // --- Position (2D Focus) ---

        /// <summary>
        /// The logical X-coordinate on the map grid.
        /// </summary>
        /// <remarks>This is the 2D grid position, agnostic of screen size/zoom.</remarks>
        public double X { get; set; }

        /// <summary>
        /// The logical Y-coordinate on the map grid.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The logical elevation of the token (0 for ground level). 
        /// Crucial for isometric view, but defaults to 0 in 2D.
        /// </summary>
        public int Elevation { get; set; } = 0;


        // --- Visuals and Size ---

        /// <summary>
        /// Path or URI to the image file used for the token (can be a simple shape indicator).
        /// </summary>
        public required string ImagePath { get; set; }

        /// <summary>
        /// The scaling factor for the token image (1.0 is default size).
        /// </summary>
        public double Scale { get; set; } = 1.0;

        /// <summary>
        /// A string representing the token type (Player, Opponent, NPC).
        /// </summary>
        public string TokenType { get; set; } = "Player";
    }
}
