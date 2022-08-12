using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using YunakApp.Interface;
using YunakApp.Repository;
using YunakApp.Services;

namespace YunakApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Класс базы данных.
        /// </summary>
        public MockDataStore DataStore => DependencyService.Get<MockDataStore>();

        public readonly ICategoryRepository _categoryRepository;
        public readonly IOperationRepository _operationRepository;
        public readonly IUserRepository _userRepository;

        public BaseViewModel()
        {
            _categoryRepository = new CategoryRepository(DataStore);
            _operationRepository = new OperationRepository(DataStore);
            _userRepository = new UserRepository(DataStore);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
