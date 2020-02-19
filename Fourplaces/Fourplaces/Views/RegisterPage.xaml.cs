using Storm.Mvvm.Forms;
using Fourplaces.ViewModels;

namespace Fourplaces.Views
{
    public partial class RegisterPage : BaseContentPage
    {
        public RegisterPage()
        {
            BindingContext = new RegisterPageViewModel();
            InitializeComponent();
        }
    }
}