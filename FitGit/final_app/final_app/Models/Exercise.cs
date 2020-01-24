using System;
using System.Collections.Generic;
using System.Text;

namespace final_app.Models
{
    public class Exercise   // Exercises that the user can select from
    {
        public Exercise()//default constructor
        {
            Name = "";
            MET = 0;
            Intensity = "Low";
        }
        public Exercise(string n, double M)//constructor for name and met
        {
            Name = n;
            MET = M;
            Intensity = "Low";
        }

        public double ExerciseTime//time of exercise
        {
            get;
            set;
        }

        public string Name// name of exercise
        {
            get;
            set;
        }

        public double MET//met of exercise
        {
            get;
            set;
        }

        public string Intensity//intensity lvl of exercise
        {
            get;
            set;
        }

        public double CaloriesVal//calroies val of exercise
        {
            get;
            set;
        }

        public double intMinutes//intensity minutes of exercise
        {
            get;
            set;
        }

        public double Calories(double Min, double Weight)//calculate calories burned for exercise
        {
            double cals = 0;
            if (this.Intensity == "High")
            {
                cals = (this.MET * Min * 3.5 * (Weight/2.2)) / 200;
                cals = cals * 1.25;
            }
            else if (this.Intensity == "Medium")
            {
                cals = (this.MET * Min * 3.5 * (Weight / 2.2)) / 200;
            }
            else
            {
                cals = (this.MET * Min * 3.5 * (Weight / 2.2)) / 200;
                cals = cals * 0.75;
            }
            CaloriesVal = cals;
            return cals;
        }
        public double IntMins(double min)//calculate intensity minutes of exercise
        {
            double mins = 0;
            if (this.Intensity == "High")
            {
                mins = min;
            }
            else if (this.Intensity == "Medium")
            {
                mins = 0.75 * min;
            }
            else
            {
                mins = 0.5 * min;
            }
            intMinutes = mins;
            return mins;
        }
    }
}
