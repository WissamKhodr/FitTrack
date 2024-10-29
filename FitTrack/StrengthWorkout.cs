using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack
{
    public class StrengthWorkout : Workout
    {
        private int _repetitions;

        public int Repetitions
        {
            get => _repetitions;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Värdet måste vara positivt!");
                if (value > 1000) 
                    throw new ArgumentException("Det känns lite för högt... sluta lalla");
                _repetitions = value;
            }
        }

        public override int CalculateCaloriesBurned()
        {
            
            double hours = Duration.TotalHours;

            
            const double baseRate = 400;

            
            double repetitionFactor = Math.Min(1.5, 1.0 + (Repetitions / 1000.0));

            return (int)(baseRate * hours * repetitionFactor);
        }

        public override void Validate()
        {
            base.Validate();
            if (Repetitions <= 0)
                throw new InvalidOperationException("Värdet måste vara positivt!");
        }
    }
}
