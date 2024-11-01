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

        // antal reps måste va mellan 1-1000
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

        // räknar ut kalorier baserat på tid och reps
        public override int CalculateCaloriesBurned()
        {
            double tim = Duration.TotalHours;
            const double basRa = 400;
            double reps = Math.Min(1.5, 1.0 + (Repetitions / 1000.0));
            return (int)(basRa * tim * reps);
        }

        // kollar att antalet reps är ok
        public override void Validate()
        {
            base.Validate();
            if (Repetitions <= 0)
                throw new InvalidOperationException("Värdet måste vara positivt!");
        }
    }
}
