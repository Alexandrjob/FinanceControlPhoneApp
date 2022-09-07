using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YunakApp.Models;
using YunakApp.ViewModels;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCategoryPopupPage : PopupPage
    {
        readonly AboutViewModel _aboutView;
        public readonly Category _category;

        public EditCategoryPopupPage(){ }

        public EditCategoryPopupPage(AboutViewModel aboutView, Category category)
        {
            InitializeComponent();
            _aboutView = aboutView;
            _category = category;

            name.Text = _category.Name;
            picker.SelectedIndex = (int)_category.Type;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            if(IsNotValidation())
            {
                return;
            }

            _category.Name = name.Text;
            _category.Type = (Type)picker.SelectedIndex;
          
            _aboutView.EditCategory(_category);
            //Выходим с попапа.
            await PopupNavigation.Instance.PopAsync();
        }

        private bool IsNotValidation()
        {
            if (name.Text == default || name.Text == "" || name.Text == " ")
            {
                name.Placeholder = "Введите название";
                name.PlaceholderColor = Color.Red;

                return true;
            }

            return false;
        }

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }
    }
}