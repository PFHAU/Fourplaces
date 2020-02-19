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
    public class UpdatePasswordPageViewModel : ViewModelBase
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<INavigationService> _navigationService;

        private string _oldpassword;
        public string OldPassword
        {
            get => _oldpassword;
            set => SetProperty(ref _oldpassword, value);
        }

        private string _newpassword;
        public string NewPassword
        {
            get => _newpassword;
            set => SetProperty(ref _newpassword, value);
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

        public ICommand ModifyCommand { get; }

        public UpdatePasswordPageViewModel()
        {
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            _userService = new Lazy<IUserService>(() => DependencyService.Resolve<IUserService>());

            ModifyCommand = new Command(ModifyAction);

            IsEnableButton = true;
            ErrorBool = false;
        }

        public async void ModifyAction()
        {
            IsEnableButton = false;
            string result = await _userService.Value.ChangePassword(_oldpassword, _newpassword);

            if (result == null || result == "")
            {
                await _navigationService.Value.PopAsync();
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