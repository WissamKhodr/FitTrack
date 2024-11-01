using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitTrack
{
    public partial class UserDetailsWindow : Window
    {
        private readonly User _currentUser;
        private readonly UserManager _userManager;

        public UserDetailsWindow(User user, UserManager userManager)
        {
            InitializeComponent();
            _currentUser = user;
            _userManager = userManager;
            InitializeWindow();
        }

        // sätter nuvarande uppgifter och väljer rätt land i listan
        private void InitializeWindow()
        {
            CurrentUsernameText.Text = _currentUser.Username;
            CurrentCountryText.Text = _currentUser.Country;

            var countryItems = CountryComboBox.Items.Cast<ComboBoxItem>();
            var currentCountryItem = countryItems.FirstOrDefault(item =>
                item.Content?.ToString() == _currentUser.Country);

            if (currentCountryItem != null)
            {
                CountryComboBox.SelectedItem = currentCountryItem;
            }
        }

        // sparar alla nya uppgifter om dom är ok
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = NewUsernameInput.Text.Trim();
            string newPassword = NewPasswordInput.Password;
            string confirmPassword = ConfirmPasswordInput.Password;
            string newCountry = (CountryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // kollar att användarnamnet är ok
            if (!string.IsNullOrEmpty(newUsername) && newUsername != _currentUser.Username)
            {
                if (newUsername.Length < 3)
                {
                    MessageBox.Show("Ditt namn måste minst vara 3 karaktärer lång!",
                        "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_userManager.UserExists(newUsername))
                {
                    MessageBox.Show("Användaren finns redan ju!",
                        "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // kollar att lösenordet är ok
            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword.Length < 5)
                {
                    MessageBox.Show("Lösenordet måste minst vara 5 karaktärer lång!",
                        "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Lösenorden matchar inte!",
                        "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // uppdaterar det som ändrats
            bool hasChanges = false;

            if (!string.IsNullOrEmpty(newUsername) && newUsername != _currentUser.Username)
            {
                _currentUser.Username = newUsername;
                hasChanges = true;
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                _currentUser.Password = newPassword;
                hasChanges = true;
            }

            if (newCountry != null && newCountry != _currentUser.Country)
            {
                _currentUser.Country = newCountry;
                hasChanges = true;
            }

            if (hasChanges)
            {
                _userManager.UpdateUser(_currentUser);
                MessageBox.Show("Användarens detaljer har uppdaterats!",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.Close();
        }

        // stänger utan att spara
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
