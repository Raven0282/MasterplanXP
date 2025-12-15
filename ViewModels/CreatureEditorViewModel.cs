// --- MasterplanXP.ViewModels/CreatureEditorViewModel.cs ---

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MasterplanXP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions; // Required for ParsePower logic

namespace MasterplanXP.ViewModels
{
    public partial class CreatureEditorViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HeaderDisplay))]
        private Monster _currentCreature;

        public CreatureEditorViewModel()
        {
            _currentCreature = CreateDemoOrc();
        }

        // HELPER METHOD: Parses the full power string into its structural components
        private static CreaturePower ParsePower(string description)
        {
            var power = new CreaturePower(description);
            string normalizedDescription = description.Trim();
            string lowerDescription = normalizedDescription.ToLower();

            // --- 1. Split Header (Name/Frequency) and Body (Damage/Effect) based on the first colon ---
            var parts = normalizedDescription.Split(new[] { ':' }, 2);
            power.NameAndFrequencyDisplay = parts.Length > 0 ? parts[0].Trim() : normalizedDescription;
            power.HitAndDamageDisplay = parts.Length > 1 ? parts[1].Trim() : "";

            // --- 2. Determine Power Type and Icon (m, r, a, c, t, u) ---
            string typePrefix = power.Description.Length > 0 ? power.Description.Substring(0, 1).ToLower() : "";

            // Remove the type prefix from the display text if it exists
            if (Regex.IsMatch(power.NameAndFrequencyDisplay, @"^[mrcaut]\s", RegexOptions.IgnoreCase))
            {
                power.NameAndFrequencyDisplay = power.NameAndFrequencyDisplay.Substring(1).Trim();
            }

            switch (typePrefix)
            {
                case "m":
                    power.Type = "Melee";
                    power.IconPath = "avares://MasterplanXP/Assets/Images/melee.svg";
                    break;
                case "r":
                    power.Type = "Ranged";
                    power.IconPath = "avares://MasterplanXP/Assets/Images/ranged.svg";
                    break;
                case "a":
                    power.Type = "Area";
                    power.IconPath = "avares://MasterplanXP/Assets/Images/area.svg";
                    break;
                case "c":
                    power.Type = "Close";
                    power.IconPath = "avares://MasterplanXP/Assets/Images/close.svg";
                    break;
                case "t":
                    power.Type = "Targeted";
                    power.IconPath = "avares://MasterplanXP/Assets/Images/target.svg";
                    break;
                case "u":
                    power.Type = "Utility";
                    power.IconPath = "avares://MasterplanXP/Assets/Images/utility.svg";
                    break;
                default:
                    power.Type = "Melee";
                    power.IconPath = "avares://MasterplanXP/Assets/Images/melee.svg";
                    break;
            }

            // --- 3. Determine Frequency (for internal data, not strictly needed for this problem but good practice) ---
            if (lowerDescription.Contains("daily"))
                power.Frequency = "Daily";
            else if (lowerDescription.Contains("encounter"))
                power.Frequency = "Encounter";
            else if (lowerDescription.Contains("at-will") || lowerDescription.Contains("basic attack"))
                power.Frequency = "At-Will";
            else
                power.Frequency = "Special";


            return power;
        }


        private static Monster CreateDemoOrc()
        {
            return new Monster
            {
                // FIX 1: Set Id to Guid.NewGuid() since Monster.Id is a required Guid.
                Id = Guid.NewGuid(),

                Name = "Orc Crusher",
                Level = 3,
                Role = "Brute",
                ExperiencePoints = 150,
                SizeAndType = "Medium natural humanoid",
                MaxHitPoints = 50,
                ArmorClass = 17,
                Fortitude = 15,
                Reflex = 14,
                Will = 13,
                Speed = 6,
                Traits = "Threatening Reach: The orc can make opportunity attacks against creatures within 2 squares.",

                // FIX: Initialize Actions with CreaturePower objects using ParsePower
                Actions = new List<CreaturePower>
                {
                    ParsePower("m Greatclub (Standard; At-Will): +8 vs AC, 1d10+4 damage."),
                    ParsePower("C Cleave (Standard; Encounter): Targets two adjacent foes; +8 vs AC, 1d10+4 damage to each.")
                },

                // === D&D 4E FIELDS ===
                Skills = "Athletics +10, Intimidate +8, Perception +3",
                Resistances = "None",
                Immunities = "None",
                Alignment = "Evil",
                Languages = "Common, Giant",

                // FIX: Initialize TriggeredActions with CreaturePower objects using ParsePower
                TriggeredActions = new ObservableCollection<CreaturePower>
                {
                    ParsePower("Immediate Reaction: Vengeful Strike (When first bloodied, the orc makes one basic melee attack against the nearest enemy.)")
                },
                Sourcebook = "Monster Manual (MM 202)"
            };
        }

        [RelayCommand]
        private void LevelUp()
        {
            if (CurrentCreature.Level < 50)
            {
                CurrentCreature.Level++;
                CurrentCreature.Fortitude++;
                CurrentCreature.Reflex++;
                CurrentCreature.Will++;
                OnPropertyChanged(nameof(CurrentCreature));
            }
        }

        [RelayCommand]
        private void LevelDown()
        {
            if (CurrentCreature.Level > 1)
            {
                CurrentCreature.Level--;
                CurrentCreature.Fortitude--;
                CurrentCreature.Reflex--;
                CurrentCreature.Will--;
                OnPropertyChanged(nameof(CurrentCreature));
            }
        }

        [RelayCommand]
        private void AddAction()
        {
            // Action: adds a new CreaturePower object using ParsePower
            CurrentCreature.Actions.Add(ParsePower("m New Action Power (Standard; At-Will): +0 vs AC, 1 damage."));

            // Manually notify the ItemsControl to update 
            OnPropertyChanged(nameof(CurrentCreature.Actions));
        }

        // --- Computed Properties ---

        public string HeaderDisplay => $"{CurrentCreature.Name} (Level {CurrentCreature.Level} {CurrentCreature.Role})";

        // Combines Skills, Resistances, Immunities, etc. into a single, structured text block
        public string StatsSummary
        {
            get
            {
                var summary = $"Skills {CurrentCreature.Skills}\n";
                if (!string.IsNullOrEmpty(CurrentCreature.Resistances) && CurrentCreature.Resistances.ToLower() != "none")
                    summary += $"Resistances {CurrentCreature.Resistances}\n";
                if (!string.IsNullOrEmpty(CurrentCreature.Immunities) && CurrentCreature.Immunities.ToLower() != "none")
                    summary += $"Immunities {CurrentCreature.Immunities}\n";

                summary += $"Alignment {CurrentCreature.Alignment}\n";
                summary += $"Languages {CurrentCreature.Languages}";

                return summary;
            }
        }
    }
}