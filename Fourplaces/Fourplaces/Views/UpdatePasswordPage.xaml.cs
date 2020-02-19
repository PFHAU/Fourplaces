using Storm.Mvvm.Forms;
using Fourplaces.ViewModels;

namespace Fourplaces.Views
{
    public partial class UpdatePasswordPage : BaseContentPage
    {
        public UpdatePasswordPage()
        {
            BindingContext = new UpdatePasswordPageViewModel();
            InitializeComponent();
        }
    }
}