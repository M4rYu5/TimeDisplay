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

        /// <summary>
        /// App supported color scheme.
        /// </summary>
        /// <remarks>
        /// When changed, make sure you add/rename/remove localized names into Resources/Localization/ThemeNames <\br>
        /// Also you have to add it's resource (dictionary) to ColorPaletteFactory
        /// </remarks>
        public enum ColorScheme
        {
            OsDefault = 0,
            Dark,
            Light,
        }

        /// <summary>
        /// Return the localized name of this ColorScheme
        /// </summary>
        public static string GetLocalizedName(this ColorScheme scheme)
        {
            return Localization.ThemeNames.ResourceManager.GetString(scheme.GetName());
        }

        /// <summary>
        /// Return the name of this ColorScheme
        /// </summary>
        public static string GetName(this ColorScheme scheme)
        {
            return Enum.GetName(typeof(ColorScheme), scheme);
        }

        /// <summary>
        /// Return the scheme from a given name
        /// </summary>
        /// <returns>
        /// The color scheme, or null if not found
        /// </returns>
        public static ColorScheme? GetSchemeFromName(string name)
        {
            if (Enum.TryParse<ColorScheme>(name, out var scheme))
                return scheme;
            return null;
        }



        /// <summary>
        /// Specify a link between system (OS) themes and application color pallets. <br/>
        /// </summary>
        /// <remarks>
        /// Don't assign the ColorScheme.OsDefault to a native theme
        /// </remarks>
        public static IDictionary<OSAppTheme, ColorScheme> MapNativeThemes { get; } = new Dictionary<OSAppTheme, ColorScheme>()
        {
            {OSAppTheme.Light, ColorScheme.Light },
            {OSAppTheme.Dark, ColorScheme.Dark },
            {OSAppTheme.Unspecified, ColorScheme.Dark }
        };

        /// <summary>
        /// Return the localization values for all defiend themes / color palettes
        /// </summary>
        public static IEnumerable<(ColorScheme, string)> LocalizedPalettesValues
        {
            get
            {
                foreach (ColorScheme theme in Enum.GetValues(typeof(ColorScheme)))
                {
                    yield return (theme, theme.GetLocalizedName());
                }
            }
        }

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
