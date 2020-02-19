using Storm.Mvvm.Forms;
using Fourplaces.ViewModels;

namespace Fourplaces.Views
{
    public partial class MyAccountPage : BaseContentPage
    {
        public MyAccountPage()
        {
            BindingContext = new MyAccountPageViewModel();
            InitializeComponent();
        }
    }
}