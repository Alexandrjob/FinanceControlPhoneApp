﻿using Rg.Plugins.Popup.Pages;
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

        public AddCategoryPopupPage(ICategoryRepository categoryRepository)
        {
            InitializeComponent();
            _categoryRepository = categoryRepository;

            picker.SelectedIndex = (int)Type.consumption;
        }       

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (IsNotValidation())
            {
                return;
            }

            await _categoryRepository.AddCategoryAsync(name.Text, (Type)picker.SelectedIndex);
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