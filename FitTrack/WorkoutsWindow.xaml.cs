﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class WorkoutsWindow : Window
    {
        private readonly User _currentUser;
        private readonly UserManager _userManager;
        private ObservableCollection<Workout> _workouts;

        public WorkoutsWindow(User user, UserManager userManager)
        {
            InitializeComponent();
            _currentUser = user;
            _userManager = userManager;
            _workouts = new ObservableCollection<Workout>();
            InitializeWindow();
        }

        // fixar listan med alla träningspass
        private void InitializeWindow()
        {
            UserNameText.Text = _currentUser.Username;

            // admins ser allas träningspass
            if (_currentUser is AdminUser)
            {
                _workouts = new ObservableCollection<Workout>(_userManager.GetAllWorkouts());
            }
            else
            {
                // vanliga users ser bara sina egna
                _workouts = new ObservableCollection<Workout>(_userManager.GetUserWorkouts(_currentUser.Username));
            }

            WorkoutsListView.ItemsSource = _workouts;
        }

        // öppnar fönster för att lägga till pass
        private void AddWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            var addWorkoutWindow = new AddWorkoutWindow(_currentUser, _userManager);
            addWorkoutWindow.ShowDialog();
            RefreshWorkoutsList();
        }

        // öppnar fönster för att se detaljer
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedWorkout = WorkoutsListView.SelectedItem as Workout;
            if (selectedWorkout == null)
            {
                MessageBox.Show("Lägg till ett träningspass först!", "Fel!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var detailsWindow = new WorkoutDetailsWindow(selectedWorkout, _currentUser, _userManager);
            detailsWindow.ShowDialog();
            RefreshWorkoutsList();
        }

        // tar bort valt träningspass
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedWorkout = WorkoutsListView.SelectedItem as Workout;
            if (selectedWorkout == null)
            {
                MessageBox.Show("Lägg till ett träningspass först!", "Fel!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Är du säker på att du vill ta bort?",
                "Ja", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _userManager.RemoveWorkout(_currentUser.Username, selectedWorkout);
                RefreshWorkoutsList();
            }
        }

        // öppnar användarinställningar
        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            var userDetailsWindow = new UserDetailsWindow(_currentUser, _userManager);
            userDetailsWindow.ShowDialog();
            UserNameText.Text = _currentUser.Username;
        }

        // visar info om appen
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "FitTrack by Wizz & Zizi - Din Personlig Fitness App\n\n" +
                "• Lägg till träningspass med 'Add Workout' knappen\n" +
                "• Visa detaljer genom att trycka på ett träningspass och sedan 'Details' knappen\n" +
                "• Ta bort träningspass med 'Remove' knappen\n" +
                "• Kom åt dina användar detaljer via 'User Details' knappen\n\n" +
                "Med FitTrack kan du få precis den body typen du har alltid viljat ha!\n\n" +
                "By wizz & zizi - FitTrack\n\n" +
                "Ideas & Creativity by zizi",
                "FitTrack App",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        // söker efter träningspass
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                RefreshWorkoutsList();
                return;
            }

            var searchText = SearchBox.Text.ToLower();

            // hämtar rätt lista att söka i
            var allWorkouts = _currentUser is AdminUser
                ? _userManager.GetAllWorkouts()
                : _userManager.GetUserWorkouts(_currentUser.Username);

            // kollar typ, datum och anteckningar
            var filteredWorkouts = allWorkouts.Where(w =>
                w.Type.ToLower().Contains(searchText) ||
                w.Date.ToString().ToLower().Contains(searchText) ||
                w.Notes.ToLower().Contains(searchText)).ToList();

            _workouts = new ObservableCollection<Workout>(filteredWorkouts);
            WorkoutsListView.ItemsSource = _workouts;

            if (!filteredWorkouts.Any())
            {
                MessageBox.Show("Hittade inga träningspass! Testa något annat. För att få tillbaka\n" +
                    "alla träningspass, töm fältet och sök igen", "Inga resultat",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // loggar ut
        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        // uppdaterar listan med träningspass
        private void RefreshWorkoutsList()
        {
            if (_currentUser is AdminUser)
            {
                _workouts = new ObservableCollection<Workout>(_userManager.GetAllWorkouts());
            }
            else
            {
                _workouts = new ObservableCollection<Workout>(_userManager.GetUserWorkouts(_currentUser.Username));
            }
            WorkoutsListView.ItemsSource = _workouts;
        }
    }
}
