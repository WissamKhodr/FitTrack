using System.Data.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitTrack
{
    public partial class MainWindow : Window
    {
        public string LabelTitle { get; set; }
        public string UsernameInput { get; set; }
        public string PasswordInput { get; set; }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            // Sign in logik
            var workoutsWindow = new WorkoutsWindow();
            workoutsWindow.Show();
            this.Close();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}