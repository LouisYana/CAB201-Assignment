//Systems to be used by the class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MRRCManagement
{
    //enums gender to be used by the class
    public enum Gender { MALE = 0, FEMALE = 1, OTHER = 2 }

    //customer class
    public class Customer
    {
        //variables to be used by customer class
        public int customerID;
        private string title;
        private string firstName;
        private string lastName;
        private Gender gender;
        private DateTime DOB;

        //function to create a customer
        public Customer(int ID, string title, string firstName, string lastName, Gender gender, DateTime DOB)
        {
            //assign given parameters to variables in the class
            customerID = ID;
            this.title = title;
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.DOB = DOB;
        }

        //convert the attributes of a customer into a CSV format string
        public string ToCSVString()
        {
            return $"{customerID},{this.title},{this.firstName},{this.lastName},{this.gender},{this.DOB.ToString("dd/MM/yyyy")}";
        }

        //convert the attributes of a customer into a readable string to be printed by the application
        public override string ToString()
        {
            return $"{customerID.ToString().PadRight(4)}{this.title.PadRight(6)}{this.firstName.PadRight(15)}" +
                $"{this.lastName.PadRight(15)}{this.gender.ToString().PadRight(8)}" +
                $"{this.DOB.ToString("d/MM/yyyy").PadRight(4)}";
        }

        
    }
}
