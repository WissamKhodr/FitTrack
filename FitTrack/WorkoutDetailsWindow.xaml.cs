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
    public partial class WorkoutDetailsWindow : Window
    {
        private readonly Workout _workout;
        private readonly User _currentUser;
        private readonly UserManager _userManager;
        private bool _isEditMode = false;

        public WorkoutDetailsWindow(Workout workout, User user, UserManager userManager)
        {
            InitializeComponent();
            _workout = workout;
            _currentUser = user;
            _userManager = userManager;
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            
            DatePicker.SelectedDate = _workout.Date.Date;
            TimeInput.Text = _workout.Date.ToString("HH:mm");
            TypeInput.Text = _workout.Type;
            DurationInput.Text = _workout.Duration.TotalMinutes.ToString();
            CaloriesInput.Text = _workout.CaloriesBurned.ToString();
            NotesInput.Text = _workout.Notes;

            
            if (_workout is CardioWorkout cardioWorkout)
            {
                CardioPanel.Visibility = Visibility.Visible;
                StrengthPanel.Visibility = Visibility.Collapsed;
                DistanceInput.Text = cardioWorkout.Distance.ToString();
            }
            else if (_workout is StrengthWorkout strengthWorkout)
            {
                CardioPanel.Visibility = Visibility.Collapsed;
                StrengthPanel.Visibility = Visibility.Visible;
                RepetitionsInput.Text = strengthWorkout.Repetitions.ToString();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _isEditMode = !_isEditMode;

            
            EditButton.Content = _isEditMode ? "Sluta Redigera" : "Redigera";
            SaveButton.Visibility = _isEditMode ? Visibility.Visible : Visibility.Collapsed;

            
            DatePicker.IsEnabled = _isEditMode;
            TimeInput.IsReadOnly = !_isEditMode;
            DurationInput.IsReadOnly = !_isEditMode;
            CaloriesInput.IsReadOnly = !_isEditMode;
            NotesInput.IsReadOnly = !_isEditMode;

            if (_workout is CardioWorkout)
            {
                DistanceInput.IsReadOnly = !_isEditMode;
            }
            else if (_workout is StrengthWorkout)
            {
                RepetitionsInput.IsReadOnly = !_isEditMode;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInputs())
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

                
                _workout.Date = dateTime;
                _workout.Duration = TimeSpan.FromMinutes(duration);
                _workout.CaloriesBurned = calories;
                _workout.Notes = notes;

                
                if (_workout is CardioWorkout cardioWorkout)
                {
                    cardioWorkout.Distance = int.Parse(DistanceInput.Text);
                }
                else if (_workout is StrengthWorkout strengthWorkout)
                {
                    strengthWorkout.Repetitions = int.Parse(RepetitionsInput.Text);
                }

                
                _userManager.UpdateWorkout(_currentUser.Username, _workout);

                MessageBox.Show("Träningspass updaterad!",
                    "Klar!", MessageBoxButton.OK, MessageBoxImage.Information);

                
                _isEditMode = false;
                EditButton.Content = "Redigera";
                SaveButton.Visibility = Visibility.Collapsed;

                
                DatePicker.IsEnabled = false;
                TimeInput.IsReadOnly = true;
                DurationInput.IsReadOnly = true;
                CaloriesInput.IsReadOnly = true;
                NotesInput.IsReadOnly = true;
                if (_workout is CardioWorkout)
                {
                    DistanceInput.IsReadOnly = true;
                }
                else if (_workout is StrengthWorkout)
                {
                    RepetitionsInput.IsReadOnly = true;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Endast siffror!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Felmeddelande: {ex.Message}",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Välj ett datum!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TimeInput.Text))
            {
                MessageBox.Show("Välj en tid!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(DurationInput.Text))
            {
                MessageBox.Show("Lägg till rätt tid!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(CaloriesInput.Text))
            {
                MessageBox.Show("Lägg till rätt kalorier!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_workout is CardioWorkout && string.IsNullOrWhiteSpace(DistanceInput.Text))
            {
                MessageBox.Show("Lägg till rätt distans!",
                    "Fel!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_workout is StrengthWorkout && string.IsNullOrWhiteSpace(RepetitionsInput.Text))
            {
                MessageBox.Show("Lägg till rätt repetitioner!",
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
