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
            if (ThemeManager.app != null)
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
            // we are going to change the app acordingly, only if the theme is ses based on the system defaults
            if (CurrentTheme == ColorPalettes.ColorScheme.OsDefault)
            {
                var scheme = ColorPalettes.GetSchemeFromName(CurrentTheme.GetName()) ?? ColorPalettes.ColorScheme.OsDefault;
                // when the color is set base of the os default we should always refresh the theme.
                CurrentTheme = scheme;
            }
        }

        public static ColorPalettes.ColorScheme CurrentTheme
        {
            get
            {
                var themePrefName = Preferences.Get(themePreferenceKey, ColorPalettes.ColorScheme.OsDefault.GetName());
                return ColorPalettes.GetSchemeFromName(themePrefName) ?? ColorPalettes.ColorScheme.OsDefault;
            }
            set
            {
                Preferences.Set(themePreferenceKey, value.GetName());
                colorPaletteContainer.MergedDictionaries.Clear();
                colorPaletteContainer.MergedDictionaries.Add(ColorPaletteFactory.Get(value));
            }
        }

        private static void ApplyTheme()
        {
            string savedThemeName = Preferences.Get(themePreferenceKey, ColorPalettes.ColorScheme.OsDefault.GetName());
            var theme = ColorPalettes.GetSchemeFromName(savedThemeName);
            CurrentTheme = theme ?? ColorPalettes.ColorScheme.OsDefault;
        }

        private class BaseColorPaletteResourceDictionary : ResourceDictionary { };
    }
}
