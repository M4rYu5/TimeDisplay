using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeDisplay.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayAll : ContentPage
    {
        public DisplayAll()
        {
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent == null)
                DisplayAllVM.Dispose();
        }
    }
}