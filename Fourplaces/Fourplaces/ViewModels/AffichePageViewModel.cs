using System;
using System.Collections.Generic;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;
using Xamarin.Forms;

using Fourplaces.Services;
using Fourplaces.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;
using TD.Api.Dtos;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Fourplaces.ViewModels
{
    public class AffichePageViewModel : ViewModelBase
    {
        private Lazy<INavigationService> _navigationService;
        private Lazy<ILieuService> _lieuService;

        [NavigationParameter("Lieu")]
        public Lieu Lieu { get; set; }

        public ObservableCollection<Comment> CommentsList { get; }

        private string _nom;
        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _imageId;
        public int ImageId
        {
            get => _imageId;
            set => SetProperty(ref _imageId, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private Image _image;
        public Image Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private Position _positionn;
        public Position Position
        {
            get => _positionn;
            set => SetProperty(ref _positionn, value);
        }

        private string _pageName;
        public string PageName
        {
            get => _pageName;
            set => SetProperty(ref _pageName, value);
        }

        private Map _mapView;
        public Map MapView
        {
            get => _mapView;
            set => SetProperty(ref _mapView, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
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



        public ICommand SendMsgCommand { get; }


        public AffichePageViewModel()
        {
            _lieuService = new Lazy<ILieuService>(() => DependencyService.Resolve<ILieuService>());
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());

            CommentsList = new ObservableCollection<Comment>();

            SendMsgCommand = new Command(SendMsgAction);


            MapView = new Map();

            IsEnableButton = true;
            ErrorBool = false;



        }

        public async void SendMsgAction()
        {
            IsEnableButton = false;
            string result = await _lieuService.Value.SendMsg(Id, Message);

            if (result == null || result == "")
            {
                Message = "";
                await refresh();
            }
            else
            {
                ErrorBool = true;
                ErrorMsg = result;
            }
            IsEnableButton = true;
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);



            Nom = Lieu.Nom;
            Description = Lieu.Description;
            Position = Lieu.Position;
            ImageId = Lieu.ImageId;
            Id = Lieu.IdApi;
            


            MapView.Pins.Add(new Pin()
            {
                Position = Position,
                Label = Nom
            });

            MapView.MoveToRegion(new MapSpan(Position, 0.02, 0.02));
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Image = await _lieuService.Value.getUriImage(ImageId);
            await refresh();

        }

        public async Task refresh()
        {
            
            CommentsList.Clear();
            var commentsList = await _lieuService.Value.GetComments(Id);

            System.Diagnostics.Debug.WriteLine(commentsList.Count);
            if (commentsList is null)
            {

            }
            else
            {

                foreach (Comment comment in commentsList)
                {
                    CommentsList.Add(comment);

                }
            }
        }
    }
}