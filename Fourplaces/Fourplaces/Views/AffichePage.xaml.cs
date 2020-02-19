using Storm.Mvvm.Forms;
using Fourplaces.ViewModels;

namespace Fourplaces.Views
{

	public partial class AffichePage : BaseContentPage
    {
		public AffichePage ()
		{
            BindingContext = new AffichePageViewModel();
            InitializeComponent ();
		}
	}
}