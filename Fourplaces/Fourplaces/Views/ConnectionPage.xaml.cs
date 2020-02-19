using System.ComponentModel;
using Storm.Mvvm.Forms;
using Fourplaces.ViewModels;

namespace Fourplaces.Views
{
    [DesignTimeVisible(false)]
    public partial class ConnectionPage : BaseContentPage
    {
        public ConnectionPage()
        {
            BindingContext = new ConnectionPageViewModel();
            InitializeComponent();
            
        }
    }
}