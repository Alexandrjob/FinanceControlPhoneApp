using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using YunakApp.Models;
using YunakApp.Services;

namespace YunakApp.ViewModels
{
    class AboutViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Operation> Operations { get; set; }
        public float PercentageDifferenceFloat { get; set; }

        public float progressBarVallue;
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

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public MockDataStore DataStore { get; set; }
        public Command LoadDataCommand { get; }

        public AboutViewModel()
        {
            Operations = new ObservableCollection<Operation>();
            UserGeneralInformation = new GeneralInformation();
            DataStore = new MockDataStore();
            LoadDataCommand = new Command(async () => await GetData());

            //Task.Run(async () => await GetData());
        }

        private async Task GetData()
        {
            IsRefreshing = true;

            User user = await DataStore.GetUserDataAsync();
            UserGeneralInformation = user.GeneralInformation;

            Operations.Clear();

            var operations = await DataStore.GetOperationsDataAsync();
            foreach (var item in operations)
            {
                Operations.Add(item);
            }

            IsRefreshing = false;
            await PbAsync();
        }

        private async Task PbAsync()
        {
            await Task.Run(async () => await Pb());
        }

        private async Task Pb()
        {
            for (int i = 0; i < 170; i++)
            {
                ProgressBarVallue -= 0.006f;
                Thread.Sleep(1);
            }

            PercentageDifferenceFloat = (float)(UserGeneralInformation.MonthlyConsumption / (UserGeneralInformation.MonthlyIncome / 100) / 100);
            //userGeneralInformation.PercentageDifference = (int)(UserGeneralInformation.MonthlyConsumption / (UserGeneralInformation.MonthlyIncome / 100));
            for (int i = 0; i < 250; i++)
            {
                if (ProgressBarVallue + 0.004f <= PercentageDifferenceFloat)
                {
                    ProgressBarVallue += 0.004f;
                    Thread.Sleep(1);
                }
                else
                {
                    break;
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
