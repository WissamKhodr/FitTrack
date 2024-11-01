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
    public partial class TwoFactorWindow : Window
    {
        private readonly string _generatedkod;
        private readonly User _user;
        private readonly UserManager _userManager;

        public TwoFactorWindow(User user, UserManager userManager)
        {
            InitializeComponent();
            _user = user;
            _userManager = userManager;

            // gnererar random 6-siffrig kod
            Random rnd = new Random();
            _generatedkod = rnd.Next(100000, 999999).ToString();

            GeneratedCodeText.Text = _generatedkod;
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CodeInput.Text == _generatedkod)
            {
                var workoutsWindow = new WorkoutsWindow(_user, _userManager);
                workoutsWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Fel kod! Försök igen.", "Fel!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                CodeInput.Text = "";
            }
        }
    }
}
