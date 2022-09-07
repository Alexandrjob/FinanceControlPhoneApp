using System;
using System.Drawing;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;
using YunakApp.Interface;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOperationPopupPage : PopupPage
    {
        readonly IOperationRepository _operationRepository;
        private string nameCategory;

        public AddOperationPopupPage(IOperationRepository operationRepository, string nameCurrentCategory)
        {
            InitializeComponent();

            _operationRepository = operationRepository;
            nameCategory = nameCurrentCategory;
        }

        private async void Button_ClickedAddOperation(object sender, EventArgs e)
        {
            if (IsNotValidation())
            {
                return;
            }

            await _operationRepository.AddOperationAsync(nameCategory, name.Text, Convert.ToInt32(cost.Text), dateTime.Date);

            //Выходим с попапа.
            await PopupNavigation.Instance.PopAsync();
        }

        private bool IsNotValidation()
        {
            if (name.Text == default || name.Text == "" || name.Text == " " || cost.Text == default)
            {
                name.Placeholder = "Введите название";
                name.PlaceholderColor = Color.Red;

                return true;
            }
            if (cost.Text == default)
            {
                cost.Placeholder = "введите сумму";
                cost.PlaceholderColor = Color.Red;

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