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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DisplayAllVM.RefreshCommand.Execute(this);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent == null)
                DisplayAllVM.Dispose();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var vm = ((ViewModels.ClockViewModel)((View)sender).BindingContext);
            var uri = "details?id=" + (vm.ID == 5 ? 99 : vm.ID);
            await Shell.Current.GoToAsync(uri);
        }
    }
}