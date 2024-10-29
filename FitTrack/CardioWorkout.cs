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

        public override int CalculateCaloriesBurned()
        {
            
            double hours = Duration.TotalHours;
            double averageSpeed = Distance / hours; 

            
            double baseRate;
            if (averageSpeed < 8) 
                baseRate = 300;
            else if (averageSpeed < 15) 
                baseRate = 600;
            else 
                baseRate = 800;

            return (int)(baseRate * hours);
        }

        public override void Validate()
        {
            base.Validate();
            if (Distance <= 0)
                throw new InvalidOperationException("Fel! Det måste vara positivt värde!!");
        }
    }
}
