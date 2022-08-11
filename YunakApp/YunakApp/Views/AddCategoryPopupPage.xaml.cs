using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoryPopupPage: PopupPage
    {
        public AddCategoryPopupPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}