using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// --- Create this new file: Models/CreaturePower.cs ---

namespace MasterplanXP.Models
{
    public class CreaturePower
    {
        // This is the full text the user edits (e.g., "• Melee 2 (Encounter) +12 vs AC; 1d10+6 damage.")
        public string Description { get; set; }

        // **FIX: Added properties required by the ViewModel's ParsePower method**
        public string NameAndFrequencyDisplay { get; set; }
        public string HitAndDamageDisplay { get; set; }
        public string Frequency { get; set; }

        // This property will hold the type needed for styling/logic (e.g., "Melee", "Ranged", "Daily", "Utility")
        public string Type { get; set; }

        // It provides the path the View needs for the Image Source.
        public string IconPath { get; set; }

        // This is the constructor for creating a new, blank power
        public CreaturePower(string description = "")
        {
            Description = description;

            // Initialize new properties
            NameAndFrequencyDisplay = description;
            HitAndDamageDisplay = string.Empty;
            Frequency = "At-Will";

            Type = "Melee"; // Default placeholder type
            IconPath = "avares://MasterplanXP/Assets/Images/melee.svg"; // Default placeholder path
        }
    }
}