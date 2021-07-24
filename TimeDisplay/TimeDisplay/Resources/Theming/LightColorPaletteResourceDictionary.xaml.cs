using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeDisplay.Resources.Theming
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LightColorPaletteResourceDictionary : ResourceDictionary
    {
        public LightColorPaletteResourceDictionary()
        {
            InitializeComponent();
        }
    }
}