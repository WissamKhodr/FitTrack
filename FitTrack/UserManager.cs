using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack
{
    public class UserManager
    {
        private readonly Dictionary<string, User> _users;
        private readonly Dictionary<string, List<Workout>> _userWorkouts;

        public UserManager()
        {
            _users = new Dictionary<string, User>();
            _userWorkouts = new Dictionary<string, List<Workout>>();
        }

        public bool UserExists(string username)
        {
            return _users.ContainsKey(username.ToLower());
        }

        public bool ValidateUser(string username, string password)
        {
            if (!UserExists(username))
                return false;

            return _users[username.ToLower()].Password == password;
        }

        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (UserExists(user.Username))
                throw new InvalidOperationException("Användaren finns redan ju!");

            _users[user.Username.ToLower()] = user;
            _userWorkouts[user.Username.ToLower()] = new List<Workout>();
        }

        public User? GetUser(string username)
        {
            if (!UserExists(username))
                return null;

            return _users[username.ToLower()];
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!UserExists(user.Username))
                throw new InvalidOperationException("Användaren finns inte!");

            _users[user.Username.ToLower()] = user;
        }

        public void AddWorkoutToUser(string username, Workout workout)
        {
            if (workout == null)
                throw new ArgumentNullException(nameof(workout));

            if (!UserExists(username))
                throw new InvalidOperationException("Användaren finns inte!");

            _userWorkouts[username.ToLower()].Add(workout);
        }

        public void RemoveWorkout(string username, Workout workout)
        {
            if (!UserExists(username))
                throw new InvalidOperationException("Användaren finns inte!");

            _userWorkouts[username.ToLower()].Remove(workout);
        }

        public void UpdateWorkout(string username, Workout workout)
        {
            if (!UserExists(username))
                throw new InvalidOperationException("Användaren finns inte!");

            var workouts = _userWorkouts[username.ToLower()];
            var index = workouts.FindIndex(w => w == workout);
            if (index != -1)
            {
                workouts[index] = workout;
            }
        }

        public List<Workout> GetUserWorkouts(string username)
        {
            if (!UserExists(username))
                throw new InvalidOperationException("Användaren finns inte!");

            return new List<Workout>(_userWorkouts[username.ToLower()]);
        }

        public List<Workout> GetAllWorkouts()
        {
            return _userWorkouts.Values.SelectMany(w => w).ToList();
        }
    }
}
