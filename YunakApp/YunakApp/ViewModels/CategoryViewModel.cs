using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using YunakApp.Models;

namespace YunakApp.ViewModels
{
    class CategoryViewModel : BaseViewModel, IQueryAttributable
    {
        readonly Label labelType;
        readonly Grid gridBalance;
        readonly ProgressBar progressBarBalance;

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

        public CategoryViewModel(Label label, Grid grid, ProgressBar progressBar)
        {
            labelType = label;
            gridBalance = grid;
            progressBarBalance = progressBar;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            Task.Run(async () => await LoadOperations(query));
        }

        public async Task LoadOperations(IDictionary<string, string> query)
        {
            Operations = new ObservableCollection<Operation>();

            string categoryName = HttpUtility.UrlDecode(query["name"]);
            Models.Type type = (Models.Type)Convert.ToInt32(HttpUtility.UrlDecode(query["type"]));
            var dateTimeStart = Convert.ToDateTime(query["dateTimeSs"]);
            var dateTimeEnd = Convert.ToDateTime(query["dateTimeEs"]);

            ChangePageData(categoryName, type);

            try
            {
                var operations = await DataStore.GetCategoryOperationsAsync(categoryName, type);
                //TODO:
                operations = operations.Where(o => o.Date < dateTimeEnd & o.Date > dateTimeStart).ToList();
                foreach (var item in operations)
                {
                    Operations.Add(item);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load category operations");
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
    }
}
