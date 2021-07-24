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
        public static ResourceDictionary Get(string palette)
        {
            //customized for system specifics
            return palette switch
            {
                ColorPalettes.OsDefault => Get(ColorPalettes.MapNativeThemes[Application.Current.RequestedTheme]),
                ColorPalettes.Dark => new DarkColorPaletteResourceDictionary(),
                ColorPalettes.Light => new LightColorPaletteResourceDictionary(),
                _ => throw new Exceptions.ThemeNotFoundException($"Undefined theme: \"{palette}\"."),
            };
        }
    }




}
