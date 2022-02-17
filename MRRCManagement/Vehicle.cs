using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    //enums for vehicles
    public enum VehicleGrade { LUXURY = 3, COMMERCIAL = 2, FAMILY = 1, ECONOMY = 0 }
    public enum TransmissionType { AUTOMATIC = 0, MANUAL = 1 }
    public enum FuelType { PETROL = 0, DIESEL = 1 }

    //vehicle class
    public class Vehicle
    {
        //variables to be used
        public string VehicleRego;
        private string make;
        private string model;
        private int year;
        private VehicleGrade vehicleGrade;
        private int numSeats;
        private TransmissionType transmission;
        private FuelType fuel;
        private bool GPS;
        private bool sunRoof;
        public double dailyRate;
        private string colour;

        //create a new vehicle
        public Vehicle(string registration, VehicleGrade grade, string make, string model,
        int year, int numSeats, TransmissionType transmission, FuelType fuel,
        bool GPS, bool sunRoof, double dailyRate, string colour)
        {
            //assign the parameters given to temporary variables 
            VehicleRego = registration;
            vehicleGrade = grade;
            this.make = make;
            this.model = model;
            this.year = year;
            this.numSeats = numSeats;
            this.transmission = transmission;
            this.fuel = fuel;
            this.GPS = GPS;
            this.sunRoof = sunRoof;
            this.dailyRate = dailyRate;
            this.colour = colour;
        }

        //create new vehicle but with limited information
        public Vehicle(string registration, VehicleGrade grade, string make, string model, int year)
        {
            //assign paramters given to variables
            VehicleRego = registration;
            vehicleGrade = grade;
            this.make = make;
            this.model = model;
            this.year = year;

            //defualt values 
            this.numSeats = 4;
            this.transmission = TransmissionType.MANUAL;
            this.fuel = FuelType.PETROL;
            this.GPS = false;
            this.sunRoof = false;
            this.dailyRate = 50;
            this.colour = "black";

            //if vehicle grade is family default daily rate is 80
            if (vehicleGrade == VehicleGrade.FAMILY)
            {
                this.dailyRate = 80;
            }

            //if vehicle grade is economy default transmission is automatic
            else if (vehicleGrade == VehicleGrade.ECONOMY)
            {
                this.transmission = TransmissionType.AUTOMATIC;
            }

            //if vehicle grade is luxury, defualt GPS status is true
            //default sun roof status is true and defauly daily rate is 120
            else if (vehicleGrade == VehicleGrade.LUXURY)
            {
                this.GPS = true;
                this.sunRoof = true;
                this.dailyRate = 120;
            }

            //if vehicle grade is commercial default fuel type is diesel and default daily rate is 130
            else if (vehicleGrade == VehicleGrade.COMMERCIAL)
            {
                this.fuel = FuelType.DIESEL;
                this.dailyRate = 130;
            }
        }

        //returns the attributes of a vehicle in a CSV format string
        public string ToCSVString()
        {
            return $"{VehicleRego},{vehicleGrade},{this.make},{this.model},{this.year}," +
                $"{this.numSeats},{this.transmission},{this.fuel},{this.GPS}," +
                $"{this.sunRoof},{this.dailyRate},{this.colour}";
        }

        //returns the attributes of a vehicle to be readable when printed to the console line
        public override string ToString()
        {
            return $"{VehicleRego.ToString().PadRight(14)}{vehicleGrade.ToString().PadRight(15)}" +
                $"{this.make.PadRight(12)}{this.model.PadRight(13)}{this.year.ToString().PadRight(6)}" +
                $"{this.numSeats.ToString().PadRight(9)}{this.transmission.ToString().PadRight(13)}" +
                $"{this.fuel.ToString().PadRight(7)}{this.GPS.ToString().PadRight(7)}" +
                $"{this.sunRoof.ToString().PadRight(8)}{this.dailyRate.ToString().PadRight(10)}{this.colour.PadRight(5)}";
        }

        //returns attibutes in a readable format
        public List<string> GetAttributeList()
        {
            List<string> attributes = new List<string>();
            attributes.Add(VehicleRego.ToString());
            attributes.Add(vehicleGrade.ToString());
            attributes.Add(this.make);
            attributes.Add(this.model);
            attributes.Add(this.year.ToString());
            attributes.Add($"{this.numSeats} Seater");
            attributes.Add(this.transmission.ToString());
            attributes.Add(this.fuel.ToString());
            if (this.GPS == true) { attributes.Add($"GPS"); }
            if (this.sunRoof == true) { attributes.Add($"sunroof"); }
            attributes.Add(this.colour);

            return attributes;
        }

        //returns full description of a vehicle
        public string FullDescription() 
        {
            string GPS;
            string Sun;
            if (this.GPS == true) {GPS = ",GPS"; }
            else { GPS = ""; }
            if (this.sunRoof == true) { Sun = ",Sun Roof"; }
            else { Sun = ""; }
            return $"{VehicleRego.ToString()},{vehicleGrade.ToString()},{this.make},{this.model},{this.year.ToString()}" +
                $",{this.numSeats}-Seater,{this.transmission.ToString()},{this.fuel.ToString()}{GPS}{Sun},{this.colour}";
        }
    }
}
