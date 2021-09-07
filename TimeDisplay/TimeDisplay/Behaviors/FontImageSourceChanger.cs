using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeDisplay.Behaviors
{
    /// <summary>
    /// To disply the image you should set at least the glyph and the color attached properties
    /// </summary>
    public static class FontImageSourceChanger
    {
        #region Glyph Attached Property
        public static readonly BindableProperty GlyphProperty =
            BindableProperty.CreateAttached(
                "Glyph",
                typeof(string),
                typeof(FontImageSourceChanger),
                null,
                propertyChanged: OnGlyphBehaviorChanged);

        public static string GetGlyph(BindableObject view)
        {
            return (string)view.GetValue(GlyphProperty);
        }

        public static void SetGlyph(BindableObject view, string value)
        {
            view.SetValue(GlyphProperty, value);
        }

        private static void OnGlyphBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
            {
                if (GetFontImageSource(bindable, out var fontImageSource))
                    fontImageSource.Glyph = (string)newValue;
            }
        }
        #endregion
        #region Color Attached Property
        public static readonly BindableProperty ColorProperty =
            BindableProperty.CreateAttached(
                "Color",
                typeof(Color),
                typeof(FontImageSourceChanger),
                Color.Transparent,
                propertyChanged: OnColorBehaviorChanged);

        public static Color GetColor(BindableObject view)
        {
            return (Color)view.GetValue(ColorProperty);
        }

        public static void SetColor(BindableObject view, Color value)
        {
            view.SetValue(ColorProperty, value);
        }

        private static void OnColorBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
            {
                if (GetFontImageSource(bindable, out var fontImageSource))
                    fontImageSource.Color = (Color)newValue;
            }
        }
        #endregion
        #region Size Attached Property
        public static readonly BindableProperty SizeProperty =
            BindableProperty.CreateAttached(
                "Size",
                typeof(int),
                typeof(FontImageSourceChanger),
                30,
                propertyChanged: OnSizeBehaviorChanged);

        public static int GetSize(BindableObject view)
        {
            return (int)view.GetValue(SizeProperty);
        }

        public static void SetSize(BindableObject view, int value)
        {
            view.SetValue(SizeProperty, value);
        }

        private static void OnSizeBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
            {
                if (GetFontImageSource(bindable, out var fontImageSource))
                    fontImageSource.Size = (int)newValue;
            }
        }
        #endregion
        #region FontFamily Attached Property
        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.CreateAttached(
                "FontFamily",
                typeof(string),
                typeof(FontImageSourceChanger),
                null,
                propertyChanged: OnFontFamilyBehaviorChanged);

        public static string GetFontFamily(BindableObject view)
        {
            return (string)view.GetValue(FontFamilyProperty);
        }

        public static void SetFontFamily(BindableObject view, string value)
        {
            view.SetValue(FontFamilyProperty, value);
        }

        private static void OnFontFamilyBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue)
            {
                if (GetFontImageSource(bindable, out var fontImageSource))
                    fontImageSource.FontFamily = (string)newValue;
            }
        }
        #endregion

        private static bool GetFontImageSource(BindableObject bindable, out FontImageSource fontImageSource)
        {
            var fontSource = (bindable as Image)?.Source as FontImageSource;
            
            if(fontSource != null)
            {
                fontImageSource = fontSource;
                return true;
            }

            fontSource = (bindable as BaseShellItem)?.Icon as FontImageSource;
            if(fontSource != null)
            {
                fontImageSource = fontSource;
                return true;
            }

            fontImageSource = null;
            return false;
        }
    }
}
