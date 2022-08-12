using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using YunakApp.Models;
using YunakApp.Views;

namespace YunakApp.ViewModels
{
    class CategoryViewModel : BaseViewModel, IQueryAttributable
    {
        readonly Label labelType;
        readonly Grid gridBalance;
        readonly ProgressBar progressBarBalance;

        private string nameCurrentCategory;

        private ObservableCollection<Operation> operations;

        public ObservableCollection<Operation> Operations
        {
            get => operations;
            set
            {
                operations = value;
                OnPropertyChanged(nameof(Operations));
            }
        }

        public Command AddOperationCommand { get; }

        public CategoryViewModel(Label label, Grid grid, ProgressBar progressBar)
        {
            labelType = label;
            gridBalance = grid;
            progressBarBalance = progressBar;

            AddOperationCommand = new Command(OnButtonClikedAddOperation);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            Task.Run(async () => await LoadOperations(query));
        }

        public async Task LoadOperations(IDictionary<string, string> query)
        {
            Operations = new ObservableCollection<Operation>();

            string categoryName = nameCurrentCategory = HttpUtility.UrlDecode(query["name"]);
            Models.Type type = (Models.Type)Convert.ToInt32(HttpUtility.UrlDecode(query["type"]));
            var dateTimeStart = Convert.ToDateTime(query["dateTimeSs"]);
            var dateTimeEnd = Convert.ToDateTime(query["dateTimeEs"]);

            ChangePageData(categoryName, type);

            try
            {
                var operations = await _operationRepository.GetCategoryOperationsSortedByDateAsync(categoryName, type, dateTimeStart, dateTimeEnd);

                foreach (var item in operations)
                {
                    Operations.Add(item);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load category operations");
            }
        }

        private void ChangePageData(string name, Models.Type type)
        {
            if (Convert.ToBoolean(type))
            {
                labelType.Text = "Расход";
            }
            else
            {
                labelType.Text = "Доход";
                gridBalance.IsVisible = !gridBalance.IsVisible;
                progressBarBalance.IsVisible = !progressBarBalance.IsVisible;
            }
        }

        private async void OnButtonClikedAddOperation()
        {
            var page = new AddOperationPopupPage(_operationRepository, nameCurrentCategory);

            await PopupNavigation.Instance.PushAsync(page);
        }
    }
}
