using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack
{
    public class AdminUser : User
    {
        private readonly List<(string Username, Workout Workout)> _allWorkouts;

        public AdminUser()
        {
            _allWorkouts = new List<(string Username, Workout Workout)>();
        }

        public void ManageAllWorkouts()
        {
            
        }

        
        public List<Workout> GetWorkoutsByType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Kan ej vara tomt här!");

            return _allWorkouts
                .Where(w => w.Workout.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                .Select(w => w.Workout)
                .ToList();
        }

        public List<Workout> GetWorkoutsByUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Kan ej vara tomt här!");

            return _allWorkouts
                .Where(w => w.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                .Select(w => w.Workout)
                .ToList();
        }

        public List<Workout> GetWorkoutsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Lägg till startdatum först!");

            return _allWorkouts
                .Where(w => w.Workout.Date >= startDate && w.Workout.Date <= endDate)
                .Select(w => w.Workout)
                .ToList();
        }

        public void RemoveWorkoutsByUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Kan ej vara tomt här!");

            _allWorkouts.RemoveAll(w => w.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void RemoveWorkoutsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Lägg till startdatum först");

            _allWorkouts.RemoveAll(w => w.Workout.Date >= startDate && w.Workout.Date <= endDate);
        }

        public List<string> GetAllUsersWithWorkouts()
        {
            return _allWorkouts
                .Select(w => w.Username)
                .Distinct()
                .ToList();
        }

        public WorkoutStatistics GetWorkoutStatistics()
        {
            var mostActiveUser = _allWorkouts
                .GroupBy(w => w.Username)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Finns ingen sån användare";

            return new WorkoutStatistics
            {
                TotalWorkouts = _allWorkouts.Count,
                TotalUsers = GetAllUsersWithWorkouts().Count,
                TotalCardioWorkouts = _allWorkouts.Count(w => w.Workout is CardioWorkout),
                TotalStrengthWorkouts = _allWorkouts.Count(w => w.Workout is StrengthWorkout),
                AverageCaloriesBurned = _allWorkouts.Any()
                    ? (int)_allWorkouts.Average(w => w.Workout.CaloriesBurned)
                    : 0,
                MostActiveUser = mostActiveUser
            };
        }

        public List<Workout> GetAllWorkouts()
        {
            return _allWorkouts.Select(w => w.Workout).ToList();
        }

        public void AddWorkout(string username, Workout workout)
        {
            if (workout == null)
                throw new ArgumentNullException(nameof(workout));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Kan ej vara tomt här!");

            _allWorkouts.Add((username, workout));
        }

        public void RemoveWorkout(string username, Workout workout)
        {
            if (workout == null)
                throw new ArgumentNullException(nameof(workout));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Kan ej vara tomt här!");

            var workoutToRemove = _allWorkouts.FirstOrDefault(w =>
                w.Username == username && w.Workout == workout);

            if (workoutToRemove != default)
            {
                _allWorkouts.Remove(workoutToRemove);
            }
        }
    }

    public class WorkoutStatistics
    {
        public int TotalWorkouts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCardioWorkouts { get; set; }
        public int TotalStrengthWorkouts { get; set; }
        public int AverageCaloriesBurned { get; set; }
        public required string MostActiveUser { get; set; }
    }
}
