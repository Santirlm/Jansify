using Jansify_Santiago_Ramirez.ViewModels;
using System.Windows.Controls;

namespace Jansify_Santiago_Ramirez.Views
{
    public partial class InformeView : UserControl
    {
        public InformeView()
        {
            InitializeComponent();
            DataContext = new InformeViewModel();
        }
    }
}