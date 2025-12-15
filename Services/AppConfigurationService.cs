using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MasterplanXP.Services
{
    public class AppConfigurationService : IAppConfigurationService
    {
        // CENTRALIZED FLAG: Set this boolean once for the entire application.
        private const bool IsBetaBuild = true;

        public bool IsBeta => IsBetaBuild;

        public string AppVersionString { get; }

        public AppConfigurationService()
        {
            // Calculate the version string upon initialization
            AppVersionString = CalculateVersionString();
        }

        private static string GetAssemblyVersion()
        {
            // Use Reflection to get the main assembly version
            return Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0";
        }

        private string CalculateVersionString()
        {
            string version = GetAssemblyVersion();
            string prefix = IsBetaBuild ? "BETA " : string.Empty; // Use string.Empty for stable

            return prefix + version;
        }
    }
}