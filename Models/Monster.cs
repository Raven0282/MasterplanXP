using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Import the CreaturePower model
using MasterplanXP.Models;

namespace MasterplanXP.Models
{
    // A simplified model to represent a D&D 4e Monster Stat Block
    public class Monster
    {
        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public required int Level { get; set; }
        public required string Role { get; set; }
        public required long ExperiencePoints { get; set; }
        public required string SizeAndType { get; set; }
        public required int MaxHitPoints { get; set; }
        public required int ArmorClass { get; set; }
        public required int Fortitude { get; set; }
        public required int Reflex { get; set; }
        public required int Will { get; set; }
        public required int Speed { get; set; }
        public required string Traits { get; set; } // e.g., 'Savage: When an attack hits, gain 5 temp HP.'

        // REFACTOR: Change Actions from List<string> to List<CreaturePower>
        public List<CreaturePower> Actions { get; init; } = new(); // **Fixed: Removed stray citation markers**

        // NEW PROPERTIES FOR 4E FIDELITY
        public string Skills { get; set; }
        public string Resistances { get; set; }
        public string Immunities { get; set; }
        public string Alignment { get; set; }
        public string Languages { get; set; }

        // REFACTOR: Change TriggeredActions from ObservableCollection<string> to ObservableCollection<CreaturePower>
        // Initialize to prevent null reference issues
        public ObservableCollection<CreaturePower> TriggeredActions { get; set; } = new();

        public string Sourcebook { get; set; } // e.g., "Monster Manual (MM 25)"
    }

    // Static Data Definition for the Demo
    public static class DemoData
    {
        public static Monster OrcCrusher => new Monster
        {
            Id = Guid.NewGuid(),
            Name = "Orc Crusher",
            Level = 8,
            Role = "Brute",
            ExperiencePoints = 350,
            SizeAndType = "Medium natural humanoid",
            MaxHitPoints = 108,
            ArmorClass = 20,
            Fortitude = 22,
            Reflex = 19,
            Will = 19,
            Speed = 6,
            Traits = "Orc Endurance: When reduced to 0 HP, the Orc is instead reduced to 1 HP. Ends after one short rest.",

            // Actions must be CreaturePower objects
            Actions = new List<CreaturePower>
            {
                new CreaturePower("Mace (Standard, Weapon): +13 vs AC. Hit: 2d8 + 6 damage."),
                new CreaturePower("Furious Assault (Standard, Close Burst 1): +11 vs Fort. Hit: 2d8 + 4 damage, and the target is dazed (save ends)."),
                new CreaturePower("Wild Charge (Move Action): The Orc shifts 3 squares and gains temporary hit points equal to its level.")
            },
            TriggeredActions = new ObservableCollection<CreaturePower>(),
            Sourcebook = "Monster Manual (MM 25)"
        };
    }
}