using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;
using YunakApp.Interface;
using YunakApp.Models;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoryPopupPage: PopupPage
    {
        readonly ICategoryRepository _categoryRepository;

        Type type;

        public AddCategoryPopupPage(ICategoryRepository categoryRepository)
        {
            InitializeComponent();
            _categoryRepository = categoryRepository;
        }       

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await _categoryRepository.AddCategoryAsync(name.Text, type);

            //Выходим с попапа.
            await PopupNavigation.Instance.PopAsync();
        }
       
        private void picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            type = (Type)picker.SelectedIndex;
        }

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }
    }
}