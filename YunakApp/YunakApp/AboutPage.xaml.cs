
using Xamarin.Forms;
using YunakApp.ViewModels;

namespace YunakApp
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            BindingContext = new AboutViewModel();

            ProgressBar progressBar = new ProgressBar { Progress = 0.5f };
            progressBar.ProgressTo(0.75, 1500, Easing.Linear);
        }
    }
}