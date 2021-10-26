using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace TimeDisplay.Resources.Theming
{

    /// <summary>
    /// Use this class to retrive any color palette, <br/>
    /// When adding a new color palette define it into ColorPalete enum, into ThemeNames.resx file in Localization folder, and in savedThemes dictionary
    /// </summary>
    public static class ColorPaletteFactory
    {
        /// <summary>
        /// Get a specific color palette/theme
        /// </summary>
        /// <param name="palette">Palette name</param>
        /// <returns></returns>
        public static ResourceDictionary Get(ColorPalettes.ColorScheme scheme)
        {
            //customized for system specifics
            return scheme switch
            {
                ColorPalettes.ColorScheme.OsDefault => Get(ColorPalettes.MapNativeThemes[Application.Current.RequestedTheme]),
                ColorPalettes.ColorScheme.Dark => new DarkColorPaletteResourceDictionary(),
                ColorPalettes.ColorScheme.Light => new LightColorPaletteResourceDictionary(),
                _ => throw new Exceptions.ThemeNotFoundException($"Undefined theme: \"{scheme.GetName()}\"."),
            };
        }
    }




}
