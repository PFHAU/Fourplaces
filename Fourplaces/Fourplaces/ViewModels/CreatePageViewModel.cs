using System;
using System.Collections.Generic;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Fourplaces.Services;
using Fourplaces.Models;
using Xamarin.Essentials;
using Map = Xamarin.Forms.Maps.Map;
using System.Threading.Tasks;

namespace Fourplaces.ViewModels
{
    public class CreatePageViewModel : ViewModelBase
    {

        private Lazy<INavigationService> _navigationService;
        private Lazy<ILieuService> _lieuService;


        [NavigationParameter("Lieu")]
        public Lieu Lieu { get; set; }

        private string _nom;
        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private Image _imagetake;
        public Image ImageTake
        {
            get => _imagetake;
            set => SetProperty(ref _imagetake, value);
        }

        private Position _position;
        public Position Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private Map _mapView;
        public Map MapView
        {
            get => _mapView;
            set => SetProperty(ref _mapView, value);
        }

        private string _pageName;
        public string PageName
        {
            get => _pageName;
            set => SetProperty(ref _pageName, value);
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

        public ICommand ValidateCommand { get; }
        public ICommand GetLocalitationCommand { get; }
        public ICommand TakePictureCommand { get; }
        public ICommand PickPictureCommand { get; }

        public CreatePageViewModel()
        {
            _lieuService = new Lazy<ILieuService>(() => DependencyService.Resolve<ILieuService>());
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());

            ValidateCommand = new Command(ValidateAction);
            GetLocalitationCommand = new Command(GetLocalisationAction);
            TakePictureCommand = new Command(TakePictureAction);
            PickPictureCommand = new Command(PickPictureAction);

            Position = new Position(0, 0);

            MapView = new Map();

            IsEnableButton = true;
            ErrorBool = false;

        }

        public override async Task OnResume()
        {
            await base.OnResume();

            GetLocalisationAction();

            MapView.MoveToRegion(new MapSpan(Position, 0.02, 0.02));
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);

            PageName = "Création";
        }

        private async void TakePictureAction()
        {

        }

        private async void PickPictureAction()
        {

        }

        private async void OnTheMapAction()
        {
            MapView.Pins.Clear();

            MapView.Pins.Add(new Pin()
            {
                Position = Position,
                Label = "Position"
            });
        }

        private async void GetLocalisationAction()
        {
            var location = await Geolocation.GetLocationAsync();
            Position = new Position(location.Latitude, location.Longitude);

            MapView.Pins.Clear();

            MapView.Pins.Add(new Pin()
            {
                Position = Position,
                Label = "Position Actuelle"
            });
        }

        private async void ValidateAction()
        {
            IsEnableButton = false;
            var lieu = new Lieu(Nom, Description, ImageSource, Position);

            var result = await _lieuService.Value.CreateLieu(lieu);

            if(result == null || result == "")
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
