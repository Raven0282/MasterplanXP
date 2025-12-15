using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterplanXP.Models
{
    /// <summary>
    /// The comprehensive data model for an entire map session, including all tokens and settings.
    /// This is the structure used for all serialization (Binary/JSON/XML).
    /// </summary>
    public class MapData
    {
        // --- Map Properties ---
        public required string MapImagePath { get; set; }
        public double GridCellSize { get; set; }
        public bool IsIsometricView { get; set; }

        // --- Content Collections ---

        /// <summary>
        /// Collection of all token data on the map.
        /// </summary>
        public required List<TokenData> Tokens { get; set; }

        /// <summary>
        /// Collection of all zone data on the map.
        /// </summary>
        public List<ZoneData> Zones { get; set; } = new List<ZoneData>();

        // --- Combat Encounter Hook (Future) ---

        /// <summary>
        /// Data structure for the linked combat encounter (TBD).
        /// </summary>
        public CombatEncounterData? CombatEncounter { get; set; }

        // --- Tiling Data (Future) ---

        /// <summary>
        /// Data describing aggregated map tiles if the map is not a single image.
        /// </summary>
        public List<MapTileData> MapTiles { get; set; } = new List<MapTileData>();
    }

    // --- Placeholder Data Models (To be defined later) ---
    // public class ZoneData { /* TBD: Shape, Color, Coordinates, Details */ } -- Defined in its own data model
    public class CombatEncounterData { /* TBD: Initiative, Statuses, Participants */ }
    public class MapTileData { /* TBD: TileSource, Position, Rotation */ }
}