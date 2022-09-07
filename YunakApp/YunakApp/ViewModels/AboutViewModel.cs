using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using YunakApp.Models;
using YunakApp.Views;
using System.Linq;
using System.Collections.Generic;
using Type = YunakApp.Models.Type;
using System.Diagnostics;

namespace YunakApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        #region Property
        readonly EnterPeroidPopupPage EnterPeroidPopupPage;

        public Type type;
        public ObservableCollection<Category> Categories { get; set; }

        private float progressBarVallue;
        public float ProgressBarVallue
        {
            get => progressBarVallue;
            set
            {
                if (value > 1)
                {
                    progressBarVallue = 1;
                    return;
                }
                else if (value < 0)
                {
                    progressBarVallue = 0;
                    return;
                }

                progressBarVallue = value;
                OnPropertyChanged(nameof(ProgressBarVallue));
            }
        }

        private GeneralInformation userGeneralInformation;
        public GeneralInformation UserGeneralInformation
        {
            get => userGeneralInformation;
            set
            {
                userGeneralInformation = value;
                OnPropertyChanged(nameof(UserGeneralInformation));
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public Command LoadDataCommand { get; }
        public Command<Category> ItemTappedCommand { get; }
        public Command<Button> SwapCommand { get; }
        public Command AddCategoryCommand { get; }
        public Command<Category> EditCategoryCommand { get; }
        public Command<Category> DeleteCategoryCommand { get; }
        public Command SortByDateTimeCommand { get; }
        #endregion

        public AboutViewModel()
        {
            EnterPeroidPopupPage = new EnterPeroidPopupPage(this);
            type = Type.consumption;

            Categories = new ObservableCollection<Category>();
            UserGeneralInformation = new GeneralInformation();

            ItemTappedCommand = new Command<Category>(OnItemSelected);
            LoadDataCommand = new Command(async () => await GetData());
            SwapCommand = new Command<Button>(OnButtonClickedSwapIncomeConsumption);
            AddCategoryCommand = new Command(OnButtonClickedAddCategory);
            EditCategoryCommand = new Command<Category>(OnButtonClickedEditCategory);
            DeleteCategoryCommand = new Command<Category>(SwipeDeleteAsync);
            SortByDateTimeCommand = new Command(OnButtonSortByDateTimeAsync);
            Task.Run(async () => await GetData());
        }

        #region Commands
        private async Task GetData()
        {
            IsRefreshing = true;

            UserGeneralInformation = await _userRepository.GetGeneralInformation();

            var sortCategories = await _categoryRepository.GetCategoriesSortedByDateAsync(type, EnterPeroidPopupPage.DateTimeStart, EnterPeroidPopupPage.DateTimeEnd);
            BeautifulDisplay(sortCategories);

            IsRefreshing = false;
            await SetValueProgressBarAsync();
        }

        private async void OnItemSelected(Category category)
        {
            if (category == null)
                return;
            var nameCategory = category.Name;
            var totalMoney = category.TotalMoney.ToString();
            var typeStr = ((int)category.Type).ToString();
            //Берет данные либо дефолтные(начало и конец месяца) либо измененные в окне внесения периода.
            var dateTimeStartSort = EnterPeroidPopupPage.DateTimeStart.ToShortDateString();
            var dateTimeEndSort = EnterPeroidPopupPage.DateTimeEnd.ToShortDateString();

            var result = $"{nameof(CategoryPage)}?name={nameCategory}&totalMoney={totalMoney}&type={typeStr}&dateTimeSs={dateTimeStartSort}&dateTimeEs={dateTimeEndSort}";
            await Shell.Current.GoToAsync(result);
        }

        private async void OnButtonClickedSwapIncomeConsumption(Button button)
        {
            if (type == Type.income)
            {
                type = Type.consumption;
                button.Text = "Расходы";

                var sortCategoriesa = await _categoryRepository.GetCategoriesSortedByDateAsync(type, EnterPeroidPopupPage.DateTimeStart, EnterPeroidPopupPage.DateTimeEnd);
                BeautifulDisplay(sortCategoriesa);
                return;
            }
            else
            {
                type = Type.income;
                button.Text = "Доходы";

                var sortCategories = await _categoryRepository.GetCategoriesSortedByDateAsync(type, EnterPeroidPopupPage.DateTimeStart, EnterPeroidPopupPage.DateTimeEnd);
                BeautifulDisplay(sortCategories);
            }
        }

        private async void OnButtonClickedAddCategory()
        {
            var page = new AddCategoryPopupPage(_categoryRepository);

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnButtonClickedEditCategory(Category category)
        {
            var page = new EditCategoryPopupPage(this, category);

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnButtonSortByDateTimeAsync()
        {
            await PopupNavigation.Instance.PushAsync(EnterPeroidPopupPage);
        }

        private async void SwipeDeleteAsync(Category category)
        {
            await _categoryRepository.DeleteAsync(category);

            Categories.Remove(category);
        }
        #endregion

        private async Task SetValueProgressBarAsync()
        {
            await Task.Run(async () => await SetValueProgressBar());
        }

        private Task SetValueProgressBar()
        {
            for (int i = 0; i < 170; i++)
            {
                ProgressBarVallue -= 0.006f;
                Thread.Sleep(1);
            }

            var percentageDifferenceFloat = (double)(UserGeneralInformation.MonthlyConsumption / (UserGeneralInformation.MonthlyIncome / 100) / 100);
            for (int i = 0; i < 250; i++)
            {
                if (ProgressBarVallue + 0.004f <= percentageDifferenceFloat)
                {
                    ProgressBarVallue += 0.004f;
                    Thread.Sleep(1);
                }
                else
                {
                    break;
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Метод сортировки категорий по времени. Вызывается в EnterPeroidPopupPage, когда нажмается кнопка изменения диапазона дат.
        /// </summary>
        /// Такая реализация нужна для обновления данных при закрытии старницы выбора периода(EnterPeroidPopupPage).
        public async Task SortByDateTimeAsync()
        {
            var dateTimeStart = EnterPeroidPopupPage.DateTimeStart.Date;
            var dateTimeEnd = EnterPeroidPopupPage.DateTimeEnd.Date;

            var sortCategories = await _categoryRepository.GetCategoriesSortedByDateAsync(type, dateTimeStart, dateTimeEnd);
            BeautifulDisplay(sortCategories);
        }

        /// <summary>
        /// Метод для редактирования категории.
        /// </summary>
        /// Данные обновляются в EditCategoryPopupPage.
        public async void EditCategory(Category category)
        {
            await _categoryRepository.EditCategoryAsync(category);

            //Почему бы не сделать сортировку.
            Categories = new ObservableCollection<Category>(Categories.OrderByDescending(c => c.TotalMoney));
            OnPropertyChanged(nameof(Categories));
        }

        private void BeautifulDisplay(List<Category> sortCategories)
        {
            foreach (var item in Categories.ToList())
            {
                //Если в сортированном списке нет категории - удалить из главного списка.
                if (!sortCategories.Contains(item))
                {
                    Categories.Remove(item);
                }
            }

            foreach (var item in sortCategories)
            {
                //Если в главном списке нет категории из сортированного списка - добавить в главный список.
                if (!Categories.Contains(item))
                {
                    Categories.Add(item);
                }
            }
        }
    }
}