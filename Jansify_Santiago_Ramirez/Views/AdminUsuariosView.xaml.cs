using Jansify_Santiago_Ramirez.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Jansify_Santiago_Ramirez.Views
{
    public partial class AdminUsuariosView : UserControl
    {
        public AdminUsuariosView()
        {
            InitializeComponent();
            DataContext = new AdminUsuariosViewModel();
        }

        private void PasswordModal_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminUsuariosViewModel vm && vm.UsuarioEdicion != null)
                vm.UsuarioEdicion.PasswordHash = PasswordModal.Password;
        }
    }
}