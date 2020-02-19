using System.ComponentModel;
using Storm.Mvvm.Forms;
using Fourplaces.ViewModels;

namespace Fourplaces.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : BaseContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
            

        }
    }
}