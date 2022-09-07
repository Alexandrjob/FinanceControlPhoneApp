using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Drawing;
using Xamarin.Forms.Xaml;
using YunakApp.Models;
using YunakApp.ViewModels;

namespace YunakApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditOperationPopupPage : PopupPage
    {
        public readonly CategoryViewModel _categoryView;
        public readonly Operation _operation;

        public EditOperationPopupPage() { }

        public EditOperationPopupPage(CategoryViewModel categoryView, Operation operation)
        {
            InitializeComponent();

            _categoryView = categoryView;
            _operation = operation;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (IsNotValidation())
            {
                return;
            }

            _operation.Name = name.Text;
            _operation.Cost = Convert.ToDouble(cost.Text);
            _operation.Date = dateTime.Date;

            _categoryView.EditOperation(_operation);
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