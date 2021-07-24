using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeDisplay.Resources.Theming
{
    public static class ThemeManager
    {
        private const string themePreferenceKey = "theme"; // Xamarin.Essential.Preferences key

        private static Application app;
        private static BaseColorPaletteResourceDictionary colorPaletteContainer = new BaseColorPaletteResourceDictionary();

        /// <summary>
        /// Initialize ThemeManager <br/>
        /// This will set a listener to OS's theme changes, and <br/>
        /// Add/Change the application's resources dictionary to proper ColorPallete
        /// </summary>
        /// <param name="app">Set application, null to remove it</param>
        public static void Init(Application app)
        {
            if(ThemeManager.app != null)
            {
                app.Resources.MergedDictionaries.Remove(colorPaletteContainer);
                app.RequestedThemeChanged -= AppRequestedThemeChanged;
            }

            ThemeManager.app = app;
            ThemeManager.app.Resources.MergedDictionaries.Add(colorPaletteContainer);
            ApplyTheme();
            app.RequestedThemeChanged += AppRequestedThemeChanged;
        }

        // when the system theme changed
        private static void AppRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            var defaultTheme = Preferences.Get(themePreferenceKey, ColorPalettes.OsDefault);
            // we are going to change the app acordingly, only if the theme is sest based on the system defaults
            if (defaultTheme == ColorPalettes.OsDefault)
                SetTheme(defaultTheme);
        }

        private static void ApplyTheme()
        {
            string savedThemeName = Preferences.Get(themePreferenceKey, ColorPalettes.OsDefault);
            SetTheme(savedThemeName);
        }

        /// <summary>
        /// Set the theme, according to the (static) ColorPalettes constant options or its Palette property
        /// </summary>
        /// <param name="themeName">Theme name (oen of the constant fields from ColorPalettes class)</param>
        /// <exception cref="Exceptions.NotInitializedException">Init function must be called first</exception>
        public static void SetTheme(string themeName)
        {
            if (!ColorPalettes.Palettes.Contains(themeName))
                throw new Exceptions.ThemeNotFoundException("Cannot set this theme: Invalid theme name");

            Preferences.Set(themePreferenceKey, themeName);
            colorPaletteContainer.MergedDictionaries.Clear();
            colorPaletteContainer.MergedDictionaries.Add(ColorPaletteFactory.Get(themeName));
        }

        private class BaseColorPaletteResourceDictionary : ResourceDictionary { };
    }
}
