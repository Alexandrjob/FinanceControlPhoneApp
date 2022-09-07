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

        public EnterPeroidPopupPage(AboutViewModel aboutView)
        {
            DateTime now = DateTime.Now;
            DateTimeStart = new DateTime(now.Year, now.Month, 1);
            DateTimeEnd = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);

            InitializeComponent();

            dateTimeStart.Date = DateTimeStart;
            dateTimeEnd.Date = DateTimeEnd;

            AboutView = aboutView;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            DateTimeStart = dateTimeStart.Date;
            DateTimeEnd = dateTimeEnd.Date;
            await AboutView.SortByDateTimeAsync();

            //Выходим с попапа.
            await PopupNavigation.Instance.PopAsync();          
        }

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }
    }
}