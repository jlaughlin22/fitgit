using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using final_app.Models;

namespace final_app
{
    public class User
    {
        public List<Exercise> ex_list;//exercise list
        public List<Exercise> ex_hist;//exercise history
        
        //public List<Exercise> ex_list
        //{
        //    get { return ex_list}
        //}

        public User()//default constructor for user
        {
            ex_list = new List<Exercise>();
            ex_hist = new List<Exercise>();
        }

        public string Password//passord setter getter
        {
            get;
            set;
        }

        public string DOB//date of birth setter getter
        {
            get;
            set;
        }

        public string UserName//username getter setter
        {
            get;
            set;
        }

        public double Weight//weight getter setter
        {
            get;
            set;
        }
    }
    
}
