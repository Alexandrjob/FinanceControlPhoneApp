using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using YunakApp.ViewModels;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterPeroidPopupPage : PopupPage
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        readonly AboutViewModel AboutView;

        public EnterPeroidPopupPage(AboutViewModel aboutview)
        {
            InitializeComponent();

            DateTime now = DateTime.Now;
            DateTimeStart = new DateTime(now.Year, now.Month, 1);
            DateTimeEnd = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);

            AboutView = aboutview;
        }

        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            DateTimeStart = dateTimeStart.Date;
            DateTimeEnd = dateTimeEnd.Date;
            await AboutView.SortByDateTimeAsync();

            //Выходим с попапа.
            await PopupNavigation.Instance.PopAsync();          
        }
    }
}