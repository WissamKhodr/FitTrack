using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack
{
    public class CardioWorkout : Workout
    {
        private int _distance;

        // måste vara mer än 0 och mindre än 1000
        public int Distance
        {
            get => _distance;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Värdet måste vara positivt!!");
                if (value > 1000)
                    throw new ArgumentException("Det känns för högt... sluta lalla.");
                _distance = value;
            }
        }

        // räknar ut ungefär hur många kalorier man bränt
        public override int CalculateCaloriesBurned()
        {
            double hours = Duration.TotalHours;
            double averageSpeed = Distance / hours;

            double baseRate;
            if (averageSpeed < 8) // gång typ
                baseRate = 300;
            else if (averageSpeed < 15) // jogging typ
                baseRate = 600;
            else // löpning typ
                baseRate = 800;
            return (int)(baseRate * hours);
        }

        // kolla att allt ser ok ut
        public override void Validate()
        {
            base.Validate();
            if (Distance <= 0)
                throw new InvalidOperationException("Fel! Det måste vara positivt värde!!");
        }
    }
}
