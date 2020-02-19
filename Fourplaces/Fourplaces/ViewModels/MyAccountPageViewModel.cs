using System;
using System.Collections.Generic;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;
using Xamarin.Forms;
using Fourplaces.Views;
using Fourplaces.Services;
using Fourplaces.Models;
using Xamarin.Essentials;
using MonkeyCache.SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Fourplaces.ViewModels
{
    public class MyAccountPageViewModel : ViewModelBase
    {
        private Lazy<INavigationService> _navigationService;
        private Lazy<ILieuService> _lieuService;
        private Lazy<IUserService> _userService;

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set {
                SetProperty(ref _email, value);
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
                OnPropertyChanged("ModifyCommand");
                SetProperty(ref _isEnableButton, value);
            }
        }

        public User User { get; set; }

        public ICommand ModifyCommand { get; }
        public ICommand ModifyPasswordCommand { get; }


        public MyAccountPageViewModel()
        {
            _lieuService = new Lazy<ILieuService>(() => DependencyService.Resolve<ILieuService>());
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            _userService = new Lazy<IUserService>(() => DependencyService.Resolve<IUserService>());

            ModifyCommand = new Command(ModifyAction);
            ModifyPasswordCommand = new Command(ModifyPasswordAction);

            User = new User();

            IsEnableButton = true;
            ErrorBool = false;

            Initialize(null);
        }

        public async override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);

            User = await _userService.Value.Me();

            FirstName = User.FirstName;
            LastName = User.LastName;
            Email = User.Email;

        }

        public async void ModifyAction()
        {

            IsEnableButton = false;
            string result = await _userService.Value.ChangeMe(_firstName, _lastName);

            ErrorMsg = result;
            IsEnableButton = true;
        }

        public async void ModifyPasswordAction()
        {
            IsEnableButton = false;
            await _navigationService.Value.PushAsync<UpdatePasswordPage>();
            IsEnableButton = true;
        }
    }
}