//Systems to be used by the class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace MRRCManagement
{
    public class CRM
    {
        //private variables to be used
        private List<Customer> customers = new List<Customer>();
        private string crmFile;

       
        public CRM(string crmFile)
        {

            CultureInfo provider = CultureInfo.InvariantCulture;
            this.crmFile = crmFile;

            //load customers from file
            LoadFromFile();
            
        }

        //function that add new customer to the list
        public bool AddCustomer(Customer customer)
        {
            //if customer already exists don't add the customer and return false
            if (customers.Contains(customer))
            {
                return false;
            }

            //if customer dosent already exist then add the customer and return true
            else
            {
                customers.Add(customer);
                return true;
            }
        }

        //remove customer
        public bool RemoveCustomer(int ID, Fleet fleet)
        {
            //if ID is currently renting then cannot remove customer
            if (fleet.IsRenting(ID))
            {
                return false;
            }
            //if ID is not currently renting and is in the list
            else if (customers.Any(x => x.customerID == ID))
            {
                
                //find customer with that ID
                Customer result = customers.Find(x => x.customerID == ID);

                //remove the customer
                customers.Remove(result);
                return true;
            }
            //if ID does not exist in the list, return false
            else
            {
                return false;
            }
        }

        //retruns a list of customers
        public List<Customer> GetCustomers()
        {
            //return a list of customers
            return customers;
        }

        //returns a customer with the ID inputed
        public Customer GetCustomer(int ID)
        {
            //find customer with associated with the ID and return that customer
            Customer result = customers.Find(x => x.customerID == ID);
            return result;
        }

        //returns a boolean value if customer ID exists or not
        public bool DoesCustomerExist(int ID)
        {
            //if the customer ID exists in the customer list
            if (customers.Any(x => x.customerID == ID))
            {
                return true; //return true
            }
            //else if the customer ID doesnt exist in the list return false
            else
            {
                return false;
            }
        }

        //saves customers to a CSV file
        public void SaveToFIle()
        {
            //open FileStream and StreamWriter
            FileStream outFile = new FileStream(crmFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(outFile);

            //write to the file the headings of each column
            writer.WriteLine("ID,Title,firstNames,lastName,Gender,DOB");

            //for each customer in the list
            foreach (Customer person in customers)
            {
                //write to the file the customer in CSV format
                writer.WriteLine(person.ToCSVString());
            }

            //close the writer and outFile
            writer.Close();
            outFile.Close();
        }

        //loads all customers from a CSV file
        public void LoadFromFile()
        {
            //clear the list of customers before adding new ones
            customers.Clear();

            //open the file stream and stream reader
            FileStream inFile = new FileStream(crmFile, FileMode.Open);
            StreamReader reader = new StreamReader(inFile);

            int index = 0;//counter to be used by the while loop

            //while loop to loop until it reaches the last line from the reader
            while (!reader.EndOfStream)
            {
                //temporary assign the current line to a string variable
                string thisLine = reader.ReadLine();

                //the first line in the CSV file is the headings of the attributes so skip it
                if (index != 0)
                {
                    //split the string by the ',' character and assign them to a string array
                    string[] values = thisLine.Split(',');

                    //add attribute to a new customer
                    customers.Add(new Customer(int.Parse(values[0]), values[1], values[2], values[3],
                        (Gender)Enum.Parse(typeof(Gender), values[4].ToUpper()), DateTime.Parse(values[5])));
                }
                index++; //iterate the counter
            }

            //close reader and inFile
            reader.Close();
            inFile.Close();
        }
    }
}
