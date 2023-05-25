﻿using System.ComponentModel;
using ClientApplication.Utils;

namespace ClientApplication.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected readonly TaskGraphProvider TaskGraphProvider = TaskGraphProvider.GetInstance();

        public INavigationService NavigationService;
        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
