using Xamarin.Forms;
using Storm.Mvvm;
using Fourplaces.Services;
using Fourplaces.Views;
using MonkeyCache.SQLite;

namespace Fourplaces
{
    public partial class App : MvvmApplication
    {

        public App() : base(() => new ConnectionPage())
        {
            Barrel.ApplicationId = "Fourplaces2165316ID";
            InitializeComponent();
            DependencyService.Register<IUserService, UserService>();
            DependencyService.Register<ILieuService, LieuService>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}