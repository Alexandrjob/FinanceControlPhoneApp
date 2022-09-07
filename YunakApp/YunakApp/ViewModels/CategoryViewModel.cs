using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using YunakApp.Models;
using YunakApp.Views;

namespace YunakApp.ViewModels
{
    public class CategoryViewModel : BaseViewModel, IQueryAttributable
    {
        //Поля нужны чтобы их скрыть или нет(Одна станица на все типы категорий).
        readonly Label labelType;
        readonly Grid gridBalance;

        public readonly Category _category;
        /// <summary>
        /// Остаток.
        /// </summary>
        public double rest;

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
        public Command<Operation> EditOperationCommand { get; set; }
        public Command<Operation> DeleteOperationCommand { get; }

        public CategoryViewModel() { }

        public CategoryViewModel(Label label, Grid grid)
        {
            labelType = label;
            gridBalance = grid;

            _category = new Category();
            Operations = new ObservableCollection<Operation>();

            AddOperationCommand = new Command(OnButtonClikedAddOperation);
            EditOperationCommand = new Command<Operation>(OnButtonClikedEditOperation);
            DeleteOperationCommand = new Command<Operation>(SwipeDeleteAsync);
        }

        #region Commands
        private async void OnButtonClikedAddOperation()
        {
            var page = new AddOperationPopupPage(_operationRepository, _category.Name);

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnButtonClikedEditOperation(Operation operation)
        {
            var page = new EditOperationPopupPage(this, operation);

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void SwipeDeleteAsync(Operation operation)
        {
            await _operationRepository.DeleteAsync(operation);

            Operations.Remove(operation);
        }
        #endregion 

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            Task.Run(async () => await LoadOperations(query));
        }

        public async Task LoadOperations(IDictionary<string, string> query)
        {
            _category.Name = HttpUtility.UrlDecode(query["name"]);
            _category.Type = (Models.Type)Convert.ToInt32(HttpUtility.UrlDecode(query["type"]));
            rest = Convert.ToDouble(HttpUtility.UrlDecode(query["totalMoney"]));
            var dateTimeStart = Convert.ToDateTime(query["dateTimeSs"]);
            var dateTimeEnd = Convert.ToDateTime(query["dateTimeEs"]);

            ChangePageData();

            var operations = await _operationRepository.GetCategoryOperationsSortedByDateAsync(_category.Name, _category.Type, dateTimeStart, dateTimeEnd);

            foreach (var item in operations)
            {
                //TODO:Пока пусть будет, удалю как будет готовый билд.
                if (item.Name == default)
                {
                    item.Name = item.Category.Name;
                }

                Operations.Add(item);
            }
        }

        private void ChangePageData()
        {
            if (_category.Type == Models.Type.income)
            {
                labelType.Text = "Расход";
            }
            else
            {
                labelType.Text = "Доход";
                gridBalance.IsVisible = !gridBalance.IsVisible;
                //progressBarBalance.IsVisible = !progressBarBalance.IsVisible;
            }
        }

        /// <summary>
        /// Метод для редактирования операции.
        /// </summary>
        /// Данные обновляются в EditOperationPopupPage.
        public async void EditOperation(Operation operation)
        {
            await _operationRepository.EditOperationAsync(operation);

            //Почему бы не сделать сортировку.
            Operations = new ObservableCollection<Operation>(Operations.OrderByDescending(c => c.Cost));
            OnPropertyChanged(nameof(Operations));
        }
    }
}
