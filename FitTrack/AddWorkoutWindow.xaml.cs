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
    public partial class AddWorkoutWindow : Window
    {
        private readonly User _currentUser;
        private readonly UserManager _userManager;

        public AddWorkoutWindow(User user, UserManager userManager)
        {
            InitializeComponent();
            _currentUser = user;
            _userManager = userManager;
            InitializeWindow();
        }

        private void InitializeWindow()
        {
           
            DatePicker.SelectedDate = DateTime.Today;

            
            WorkoutTypeComboBox.SelectedIndex = 0;
        }

        private void WorkoutTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = (WorkoutTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            
            if (selectedType == "Cardio")
            {
                CardioPanel.Visibility = Visibility.Visible;
                StrengthPanel.Visibility = Visibility.Collapsed;
            }
            else if (selectedType == "Strength")
            {
                CardioPanel.Visibility = Visibility.Collapsed;
                StrengthPanel.Visibility = Visibility.Visible;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!ValidateCommonFields())
                    return;

                
                DateTime date = DatePicker.SelectedDate ?? DateTime.Today;
                TimeSpan time;
                if (!TimeSpan.TryParse(TimeInput.Text, out time))
                {
                    MessageBox.Show("Lägg till rätt tid!",
                        "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime dateTime = date.Add(time);
                int duration = int.Parse(DurationInput.Text);
                int calories = int.Parse(CaloriesInput.Text);
                string notes = NotesInput.Text;

                
                var workoutType = (WorkoutTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                Workout workout;

                if (workoutType == "Cardio")
                {
                    if (string.IsNullOrWhiteSpace(DistanceInput.Text))
                    {
                        MessageBox.Show("Lägg till något här!",
                            "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int distance = int.Parse(DistanceInput.Text);
                    workout = new CardioWorkout
                    {
                        Date = dateTime,
                        Type = "Cardio",
                        Duration = TimeSpan.FromMinutes(duration),
                        CaloriesBurned = calories,
                        Notes = notes,
                        Distance = distance
                    };
                }
                else 
                {
                    if (string.IsNullOrWhiteSpace(RepetitionsInput.Text))
                    {
                        MessageBox.Show("Lägg till repetitioner!",
                            "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int repetitions = int.Parse(RepetitionsInput.Text);
                    workout = new StrengthWorkout
                    {
                        Date = dateTime,
                        Type = "Strength",
                        Duration = TimeSpan.FromMinutes(duration),
                        CaloriesBurned = calories,
                        Notes = notes,
                        Repetitions = repetitions
                    };
                }

                
                _userManager.AddWorkoutToUser(_currentUser.Username, workout);

                MessageBox.Show("Lagt till träningspass!",
                    "KLART!", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Endast siffror!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fel meddelande: {ex.Message}",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateCommonFields()
        {
            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Lägg till ett datum",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TimeInput.Text))
            {
                MessageBox.Show("Lägg till en tid",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(DurationInput.Text))
            {
                MessageBox.Show("Lägg till något här!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(CaloriesInput.Text))
            {
                MessageBox.Show("Lägg till något här!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
