using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using YunakApp.Models;

namespace YunakApp.ViewModels
{
    class CategoryViewModel : BaseViewModel, IQueryAttributable
    {
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

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string name = HttpUtility.UrlDecode(query["name"]);
            Models.Type type = (Models.Type)Convert.ToInt32(HttpUtility.UrlDecode(query["type"]));
            Task.Run(async () => await LoadOperations(name, type));
        }

        public async Task LoadOperations(string name, Models.Type type)
        {
            Operations = new ObservableCollection<Operation>();

            try
            {
                var operations = await DataStore.GetCategoryOperations(name, type);
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
    }
}
