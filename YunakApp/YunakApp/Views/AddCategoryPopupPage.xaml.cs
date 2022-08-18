using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
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
            //Для первой заглявной буквы.
            //получаем TextInfo для русского языка
            //var textInfo = new CultureInfo("ru-RU").TextInfo;
            //преобразуем текст
            //var capitalizedText = textInfo.ToTitleCase(textInfo.ToLower(text));
            if(name.Text == default|| name.Text == "" || name.Text == " ")
            {
                name.Placeholder = "Введите название";
                name.PlaceholderColor = Color.Red;

                return;
            }

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