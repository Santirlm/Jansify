using Jansify_Santiago_Ramirez.ViewModels;
using System.Windows;

namespace Jansify_Santiago_Ramirez.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}