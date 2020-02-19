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
    class RegisterPageViewModel : ViewModelBase
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<INavigationService> _navigationService;

        public User User { get; set; }

        private string _firstname;
        public string FirstName
        {
            get => _firstname;
            set => SetProperty(ref _firstname, value);
        }

        private string _lastname;
        public string LasteName
        {
            get => _lastname;
            set => SetProperty(ref _lastname, value);
        }

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


        public ICommand RegisterCommand { get; }

        public RegisterPageViewModel()
        {
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            _userService = new Lazy<IUserService>(() => DependencyService.Resolve<IUserService>());

            RegisterCommand = new Command(RegisternAction);

            IsEnableButton = true;
            ErrorBool = false;
        }

        public async void RegisternAction()
        {
            IsEnableButton = false;
            string result = await _userService.Value.Register(_email,_firstname,_lastname, _password);

            if (result == null || result == "")
            {
                await _navigationService.Value.PushAsync<MainPage>();
            }
            else
            {
                ErrorMsg = result;
                ErrorBool = true;
            }
            IsEnableButton = true;
        }

    }
}
