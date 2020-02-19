using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Services;
using Fourplaces.Models;
using Fourplaces.Services;
using Fourplaces.Views;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Fourplaces.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly Lazy<ILieuService> _lieuService;
        private readonly Lazy<INavigationService> _navigationService;
        private readonly Lazy<IDialogService> _dialogService;

        public ObservableCollection<Lieu> LieuList { get; }

        private Lieu _selectedLieu;
        public Lieu SelectedLieu
        {
            get => _selectedLieu;
            set
            {
                if (SetProperty(ref _selectedLieu, value) && value != null)
                {
                    AfficheLieuAction(value);
                    SelectedLieu = null;
                }
            }
        }

        public ICommand CreateLieuCommand { get; }
        public ICommand MyAccountCommand { get; }
        public ICommand DeconnectionCommand { get; }
        

        public MainPageViewModel()
        {
            _lieuService = new Lazy<ILieuService>(() => DependencyService.Resolve<ILieuService>());
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            _dialogService = new Lazy<IDialogService>(() => DependencyService.Resolve<IDialogService>());

            LieuList = new ObservableCollection<Lieu>();

            CreateLieuCommand = new Command(CreateLieuAction);
            MyAccountCommand = new Command(MyAccountAction);
            DeconnectionCommand = new Command(DeconnextionAction);

        }



        public override async Task OnResume()
        {
            await base.OnResume();


            LieuList.Clear();
            var lieuList = await _lieuService.Value.GetAllLieux();
            if (lieuList is null)
            {
                
            }
            else
            {
                foreach (var lieu in lieuList)
                {

                    lieu.Image = await _lieuService.Value.getUriImage(lieu.ImageId);
                    LieuList.Add(lieu);

                }

            }

            //var lieuById = LieuList.ToDictionary(x => x.Id, x => x);


        }

        public async void CreateLieuAction()
        {
            await _navigationService.Value.PushAsync<CreatePage>();
        }

        public async void AfficheLieuAction(Lieu lieu)
        {
            await _navigationService.Value.PushAsync<AffichePage>(new System.Collections.Generic.Dictionary<string, object>
            {
                {"Lieu",lieu }
            });
        }

        public async void MyAccountAction()
        {
            await _navigationService.Value.PushAsync<MyAccountPage>();
        }

        public async void DeconnextionAction()
        {
            await _navigationService.Value.PopAsync();
        }
    }
}