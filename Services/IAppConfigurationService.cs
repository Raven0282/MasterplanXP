using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MasterplanXP.Services
{
    public interface IAppConfigurationService
    {
        /// <summary>
        /// Gets a value indicating whether the current build is a Beta/Development version.
        /// </summary>
        bool IsBeta { get; }

        /// <summary>
        /// Gets the calculated version string for the current application build.
        /// </summary>
        string AppVersionString { get; }
    }
}