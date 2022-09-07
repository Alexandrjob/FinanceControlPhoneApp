using Xamarin.Forms;
using YunakApp.ViewModels;

namespace YunakApp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
        }
    }
}