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
        private UserManager _userManager;

        public MainWindow()
        {
            InitializeComponent();
            _userManager = new UserManager();
            InitializeDefaultUsers();
        }

        private void InitializeDefaultUsers()
        {
            
            if (!_userManager.UserExists("admin"))
            {
                var adminUser = new AdminUser
                {
                    Username = "admin",
                    Password = "password"
                };
                _userManager.AddUser(adminUser);
            }

            
            if (!_userManager.UserExists("user"))
            {
                var regularUser = new User
                {
                    Username = "user",
                    Password = "password",
                    Country = "Sweden"
                };

                _userManager.AddUser(regularUser);

                
                var workout1 = new CardioWorkout
                {
                    Date = DateTime.Now.AddDays(-1),
                    Type = "Jogga",
                    Duration = TimeSpan.FromMinutes(30),
                    Distance = 5,
                    Notes = "Joggning På Morgonen"
                };

                var workout2 = new StrengthWorkout
                {
                    Date = DateTime.Now.AddDays(-2),
                    Type = "Styrketräning",
                    Duration = TimeSpan.FromMinutes(45),
                    Repetitions = 12,
                    Notes = "Bröst träning"
                };

                _userManager.AddWorkoutToUser("user", workout1);
                _userManager.AddWorkoutToUser("user", workout2);
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameInput.Text;
            string password = PasswordInput.Password;

            if (_userManager.ValidateUser(username, password))
            {
                var user = _userManager.GetUser(username);
                var workoutsWindow = new WorkoutsWindow(user, _userManager);
                workoutsWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Fel namn eller lösenord!", "Fel!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(_userManager);
            registerWindow.Show();
            this.Close();
        }
    }
}