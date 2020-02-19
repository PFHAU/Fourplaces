using System;
using System.Collections.Generic;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;
using Xamarin.Forms;
using MonkeyCache.SQLite;
using Fourplaces.Services;
using Fourplaces.Models;
using Xamarin.Essentials;
using TD.Api.Dtos;
using Fourplaces.Views;
using System.ComponentModel;

namespace Fourplaces.ViewModels
{
    public class ConnectionPageViewModel : ViewModelBase
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<INavigationService> _navigationService;

        public User User { get; set; }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _errorBool;
        public bool ErrorBool
        {
            get => _errorBool;
            set
            {
                OnPropertyChanged("ErrorBool");
                SetProperty(ref _errorBool, value);
            }
        }

        private string _errorMsg;
        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                OnPropertyChanged("ErrorMsg");
                SetProperty(ref _errorMsg, value);
            }
        }

        private bool _isEnableButton;
        public bool IsEnableButton
        {
            get => _isEnableButton;
            set
            {
                OnPropertyChanged("IsEnableButton");
                SetProperty(ref _isEnableButton, value);
            }
        }


        public ICommand ConnectionCommand { get; }
        public ICommand RegisterCommand { get; }

        public ConnectionPageViewModel()
        {
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            _userService = new Lazy<IUserService>(() => DependencyService.Resolve<IUserService>());
            ConnectionCommand = new Command(ConnectionAction);
            RegisterCommand = new Command(RegisternAction);

            IsEnableButton = true;
            ErrorBool = false;
        }




        public async void ConnectionAction()
        {

            IsEnableButton = false;
            string result = await _userService.Value.Login(_email, _password);

            if (result == null || result == "")
            {
                await _navigationService.Value.PushAsync<MainPage>();
            }
            else
            {
                ErrorBool = true;
                ErrorMsg = result;
            }
            IsEnableButton = true;
        }

        public async void RegisternAction()
        {
            await _navigationService.Value.PushAsync<RegisterPage>();
        }
    }
}
