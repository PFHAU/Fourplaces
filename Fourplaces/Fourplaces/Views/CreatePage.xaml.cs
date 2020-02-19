using Storm.Mvvm.Forms;
using Fourplaces.ViewModels;

namespace Fourplaces.Views
{
    public partial class CreatePage : BaseContentPage
    {
        public CreatePage()
        {
            BindingContext = new CreatePageViewModel();
            InitializeComponent();
        }
    }
}