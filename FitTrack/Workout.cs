using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack
{
    public abstract class Workout
    {
        // startvärden så inget är null
        protected Workout()
        {
            _type = string.Empty;
            _notes = string.Empty;
            _date = DateTime.Now;
            _duration = TimeSpan.Zero;
        }

        private DateTime _date;
        private string _type;
        private TimeSpan _duration;
        private int _caloriesBurned;
        private string _notes;

        // kollar att datumet inte är i framtiden
        public DateTime Date
        {
            get => _date;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Du har lagt ett datum i framtiden!");
                _date = value;
            }
        }

        // träningstypen måste va ifylld
        public string Type
        {
            get => _type;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Fältet kan inte vara tomt!!");
                _type = value;
            }
        }

        // tiden måste va mellan 0-24h
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentException("Värdet måste vara positivt!");
                if (value > TimeSpan.FromHours(24))
                    throw new ArgumentException("Du kan inte gå över 24 timmar!");
                _duration = value;
            }
        }

        // kalorierna måste va rimliga
        public int CaloriesBurned
        {
            get => _caloriesBurned;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Värdet måste vara positivt!");
                if (value > 10000)
                    throw new ArgumentException("Känns lite för högt va? Lallare...");
                _caloriesBurned = value;
            }
        }

        // anteckningar är valfritt
        public string Notes
        {
            get => _notes;
            set => _notes = value ?? string.Empty;
        }

        public abstract int CalculateCaloriesBurned();

        // kollar att alla värden är ok
        public virtual void Validate()
        {
            if (Date > DateTime.Now)
                throw new InvalidOperationException("Datumet kan inte vara i framtiden!");
            if (Duration <= TimeSpan.Zero)
                throw new InvalidOperationException("Värdet måste vara positivit!");
            if (string.IsNullOrWhiteSpace(Type))
                throw new InvalidOperationException("Lägg till vilken typ av träningspass!");
        }
    }
}
