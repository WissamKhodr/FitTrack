﻿using System;
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
    public partial class RegisterWindow : Window
    {
        private readonly UserManager _userManager;

        public RegisterWindow(UserManager userManager)
        {
            InitializeComponent();
            _userManager = userManager;
        }

        // skapar nytt konto om allt är korrekt ifyllt
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameInput.Text.Trim();
            string password = PasswordInput.Password;
            var selectedItem = CountryComboBox.SelectedItem as ComboBoxItem;
            string? country = selectedItem?.Content.ToString();

            // kollar att man valt ett land
            if (string.IsNullOrWhiteSpace(country))
            {
                MessageBox.Show("Välj ett land", "Fel!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // kollar att man fyllt i allt
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(country))
            {
                MessageBox.Show("Fyll i alla fält för att fortsätta!", "Fel!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // kollar att namnet är ledigt
            if (_userManager.UserExists(username))
            {
                MessageBox.Show("Den här användaren finns redan!", "Fel!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // sparar den nya användaren
            var newUser = new User
            {
                Username = username,
                Password = password,
                Country = country
            };

            _userManager.AddUser(newUser);
            MessageBox.Show("Du har nu skapat ett konto!", "Klart!",
                MessageBoxButton.OK, MessageBoxImage.Information);

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        // tillbaka till login
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
