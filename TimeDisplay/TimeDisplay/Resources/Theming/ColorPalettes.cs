using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace TimeDisplay.Resources.Theming
{
    /// <summary>
    /// The available color palletes. <br/>
    /// These values are used to get the localized names of each theme/color palette
    /// </summary>
    public static class ColorPalettes
    {

        public const string OsDefault = "OsDefault";
        public const string Dark = "Dark";
        public const string Light = "Light";

        /// <summary>
        /// Contains all color palettes so it is easier to enumerate through them
        /// </summary>
        public static IEnumerable<string> Palettes { get; } = new List<string> { OsDefault, Dark, Light };

        /// <summary>
        /// Specify a link between system (OS) themes and application color pallets. <br/>
        /// </summary>
        public static IDictionary<OSAppTheme, string> MapNativeThemes { get; } = new Dictionary<OSAppTheme, string>()
        {
            {OSAppTheme.Light, Light },
            {OSAppTheme.Dark, Dark },
            {OSAppTheme.Unspecified, Dark }
        };

        /// <summary>
        /// Return the localization values for all defiend themes / color palettes
        /// </summary>
        public static IEnumerable<string> LocalizedPalettesValues { get => Palettes.Select(s => Localization.ThemeNames.ResourceManager.GetString(s)); }

        /// <summary>
        /// Get the localized value for a particular paletteName
        /// </summary>
        /// <param name="paletteName">color palette name</param>
        /// <returns></returns>
        public static string GetLocalizedValue(string paletteName)
        {
            var localizedName = Localization.ThemeNames.ResourceManager.GetString(paletteName);
            if (localizedName == null)
                throw new Exceptions.ThemeLocalizatioinNotFoundException($"The localized theme name of \"{paletteName}\" wasn't found. Make sure there is a \"{paletteName}\" key in \"ThemeNames.resx\".");

            return localizedName;
        }

    }
}
