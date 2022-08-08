using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoryPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public AddCategoryPopupPage()
        {
            InitializeComponent();
        }

        //private async void OnClose(object sender, EventArgs e)
        //{
        //    await PopupNavigation.Instance.PopAsync();
        //}

        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}