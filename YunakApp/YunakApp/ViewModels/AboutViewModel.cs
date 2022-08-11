using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using YunakApp.Interface;
using YunakApp.Models;
using YunakApp.Repository;
using YunakApp.Views;

namespace YunakApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        #region Property
        readonly EnterPeroidPopupPage EnterPeroidPopupPage;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IUserRepository _userRepository;

        private Button Button;

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
        public Command SwapCommand { get; }
        public Command AddCategoryCommand { get; }
        public Command SortByDateTimecommand { get; }
        #endregion

        public AboutViewModel(Button button)
        {
            EnterPeroidPopupPage = new EnterPeroidPopupPage(this);

            _categoryRepository = new CategoryRepository(DataStore);
            _operationRepository = new OperationRepository(DataStore);
            _userRepository = new UserRepository(DataStore);

            Button = button;

            Categories = new ObservableCollection<Category>();
            UserGeneralInformation = new GeneralInformation();

            ItemTappedCommand = new Command<Category>(OnItemSelected);
            LoadDataCommand = new Command(async () => await GetData());
            SwapCommand = new Command(OnButtonClickedSwapIncomeConsumption);
            AddCategoryCommand = new Command(OnButtonClickedAddCategory);
            SortByDateTimecommand = new Command(OnButtonSortByDateTimeAsync);
            Task.Run(async () => await GetData());
        }

        private async Task GetData()
        {
            ///TODO: баг первого запуска, причина в том что вьюшка обновляется а обьекты еще пустые.
            IsRefreshing = true;

            UserGeneralInformation = await _userRepository.GetGeneralInformation();
            await _operationRepository.GetOperationsAsync();
            Categories.Clear();
            Categories = await _categoryRepository.GetCategoriesAsync();
            OnPropertyChanged(nameof(Categories));

            IsRefreshing = false;
            await SetValueProgressBarAsync();
        }

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

        private async void OnItemSelected(Category category)
        {
            if (category == null)
                return;
            var nameCategory = category.Name;
            var typeStr = ((int)category.Type).ToString();
            var dateTimeStartSort = EnterPeroidPopupPage.DateTimeStart;
            var dateTimeEndSort = EnterPeroidPopupPage.DateTimeEnd;

            var result = $"{nameof(CategoryPage)}?name={nameCategory}&type={typeStr}&dateTimeSs={dateTimeStartSort.ToShortDateString()}&dateTimeEs={dateTimeEndSort.ToShortDateString()}";
            await Shell.Current.GoToAsync(result);
        }

        private void OnButtonClickedSwapIncomeConsumption()
        {
            if (Button.Text == "Доходы")
            {
                Button.Text = "Расходы";
                return;
            }

            Button.Text = "Доходы";
        }

        private async void OnButtonClickedAddCategory(object parameter)
        {
            var page = new AddCategoryPopupPage();

            await PopupNavigation.Instance.PushAsync(page);
        }

        private async void OnButtonSortByDateTimeAsync(object obj)
        {

            await PopupNavigation.Instance.PushAsync(EnterPeroidPopupPage);
        }

        public async Task SortByDateTimeAsync()
        {
            var dateTimeStart = EnterPeroidPopupPage.DateTimeStart.Date;
            var dateTimeEnd = EnterPeroidPopupPage.DateTimeEnd.Date;

            var operations = await _operationRepository.GetOperationsAsync();

            //TODO: Это выражение нужно сразу отправлять на сервер, либо сортировать что уже есть на телефоне.
            var sortOperations = DataStore.SortOperations = operations.Where(o => o.Date < dateTimeEnd & o.Date > dateTimeStart).ToList();
            Categories.Clear();

            foreach (var item in sortOperations)
            {
                if (!Categories.Any(c => c.Name == item.Category.Name))
                {
                    Categories.Add(item.Category);
                }
            }
        }
    }
}
