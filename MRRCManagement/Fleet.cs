using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MRRCManagement
{
    public class Fleet
    {
        //variables to be used by the class
        public List<Vehicle> vehicles = new List<Vehicle>();
        private Dictionary<string, int> rentals = new Dictionary<string, int>();
        private string fleetFile;
        private string rentalFile;

        //new fleet
        public Fleet(string fleetFile, string rentalsFile)
        {
            //assign the files paths for fleet and rentals
            this.fleetFile = fleetFile;
            this.rentalFile = rentalsFile;

            //load vehicles and rentals from the csv files
            LoadVehiclesFromFile();
            LoadRentalsFromFile();
        }

        //checks if vehicle exists in the vehicle list
        public bool DoesVehicleExist(string reg)
        {
            //check if the registration of vehicle given exists in the vehicles list
            bool result = vehicles.Exists(x => x.VehicleRego == reg);

            //return the result of the check
            return result;
        }

        //display the list of rentals
        public void DisplayRentals()
        {
            //write the headings of the columns of the rentals
            Console.WriteLine("Registration".PadRight(15) + "Customer ID".PadRight(15) + "Daily rate ($)");

            //for each vehicle in rentals
            foreach (var car in rentals)
            {
                //write the vehicle and customer who rents that vehicle
                Console.Write($"{car.Key.PadRight(15)}{car.Value.ToString().PadRight(15)}");

                Vehicle rented = DisplayVehicle(car.Key);
                Console.WriteLine(rented.dailyRate);
            }
        }

        //returns vehicle which cotnains the registration parsed to it
        public Vehicle DisplayVehicle(string registration)
        {
            //find vehicle with the registration given
            Vehicle result = vehicles.Find(x => x.VehicleRego == registration);

            //return that vehicle
            return result;
        }

        //adds new vehicle to the vehicle list
        public bool AddVehicle(Vehicle vehicle)
        {
            //if vehicle already exists dont add the vehicle
            if (vehicles.Contains(vehicle))
            {
                return false;
            }

            //if vehicle doesnt currently exist add the vehicle
            else
            {
                vehicles.Add(vehicle);

                //save the new list of vehicles to the csv file
                SaveVehiclesToFile();
                return true;
            }
        }

        //removes vehicle with the registration parsed to it
        public bool RemoveVehicle(string registration)
        {
            //find vehicle with the registration given
            Vehicle result = vehicles.Find(x => x.VehicleRego == (registration));

            //if the vehicle exists in the fleet
            if (vehicles.Contains(result))
            {
                //remove vehicle and tell user it was successfully removed
                vehicles.Remove(result);
                Console.WriteLine($"deleted {registration}");
                SaveVehiclesToFile();
                return true;
            }
            //if vehicle doesnt exist in the fleet then return false
            else
            {
                return false;
            }
        }

        //returns the current list of vehicles
        public List<Vehicle> GetFleet()
        {
            //return the fleet of vehicles
            return vehicles;
        }

        //returns a lsit of vehicles
        public List<Vehicle> GetFleet(bool rented)
        {
            //create a new list of vehicles
            List<Vehicle> rented_list = new List<Vehicle>();

            //if the user wants the list of rented vehicles
            if (rented == true)
            {
                //for each vehicle in the rentals list
                foreach (var item in rentals)
                {
                    //add the rented vehicles to the lsit to be returned
                    Vehicle result = vehicles.Find(x => x.VehicleRego == item.Key);
                    rented_list.Add(result);
                }
            }

            //if user wants the list of un-rented vehicles
            else
            {
                rented_list = vehicles;

                //for each vehicle in the rented list
                foreach (var item in rentals)
                {
                    //remove the rented vehicles from the complete list of vehicles
                    Vehicle result = vehicles.Find(x => x.VehicleRego == item.Key);
                    rented_list.Remove(result);
                }
            }

            //return the list of vehicles 
            return rented_list;
        }

        //returns a boolean value based on if the vehicle parsed to it is currently renting or not
        public bool IsRented(string registration)
        {
            //if the registration given is in the rentals list
            if (rentals.ContainsKey(registration))
            {
                //return true that the vehicle is being rented
                return true;
            }

            //if the registration given is not in the rentals list
            else
            {
                //return false that the vehicle is being rented
                return false;
            }
        }

        //this method returns true if the customer parsed to it is currently renting
        //and false if customer is not currently renting
        public bool IsRenting(int customerID)
        {
            //if customer ID given is in the rentals list
            if (rentals.ContainsValue(customerID))
            {
                //return true that the customer is currently renting
                return true;
            }
            else
            {
                //return false that the customer is currently renting
                return false;
            }
        }

        //this method returns the customer ID of the customer who is renting the vehicle with registration
        public int RentedBy(string registration)
        {
            //find the customer ID of the renter that is currently renting the vehicle
            int ID = rentals.FirstOrDefault(x => x.Key == registration).Value;

            //return the ID
            return ID;
        }

        //this method rents a vehicle with registration and customer with ID
        public bool RentVehicle(string registration, int customerID)
        {
            rentals.Add(registration, customerID);
            return true;
        }

        //this method returns the vehicle in the rentals list with the registration
        public int ReturnVehicle(string registration)
        {
            //if rentals contains the vehicle
            if (rentals.ContainsKey(registration))
            {
                //find the customer that is renting that vehicle
                int ID = rentals.FirstOrDefault(x => x.Key == registration).Value;

                //remove that vehicle and return the customer ID that was renting that vehicle
                rentals.Remove(registration);
                return ID;
            }

            //if rentals does not contain that vehicle
            else
            {
                //return -1
                return -1;
            }
        }

        //this method saves all the vehicles to a csv file
        public void SaveVehiclesToFile()
        {
            //open file stream and stream writer
            FileStream outFile = new FileStream(fleetFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(outFile);

            //first write the headings of each attribute to the CSV file
            writer.WriteLine("Registration,Grade,Make,Model,Year,NumSeats,Transmission,Fuel,GPS,SunRoof,DailyRate,Colour");

            //for each vehicle in the vehicles list
            foreach (Vehicle car in vehicles)
            {
                //add the vehicle in CSV format
                writer.WriteLine(car.ToCSVString());
            }

            //close the writer and outFile
            writer.Close();
            outFile.Close();
        }

        //this method saves all rentals to the csv file
        public void SaveRentalsToFile()
        {
            //open file stream and stream writer
            FileStream outFile = new FileStream(rentalFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(outFile);

            //first, write the headings of the rentals
            writer.WriteLine("Registration,ID");

            //for each vehicle in the vehicles list
            foreach (var car in rentals)
            {
                //write the vehicle and the ID assocaited to the vehicle to the CSV file
                writer.WriteLine($"{car.Key},{car.Value}");
            }

            //close the writer and outFile
            writer.Close();
            outFile.Close();
        }

        //this method loads all the vehicles from the csv file 
        public void LoadVehiclesFromFile()
        {
            //clear the vehicles list before adding new vehicles
            vehicles.Clear();

            //open the file stream and stream reader
            FileStream inFile = new FileStream(fleetFile, FileMode.Open);
            StreamReader reader = new StreamReader(inFile);

            int index = 0; //counter to be used by the while loop

            //while it is not the end of stream
            while (!reader.EndOfStream)
            {
                //read the current line
                string thisLine = reader.ReadLine();

                //split the line into seperate values by every ',' character
                string[] readline = thisLine.Split(',');

                //the first line from the CSV file is the headings so skip that line
                if (index != 0)
                {
                    //if it is not the first line of the stream reader then add the vehicle in the stream reader
                    Vehicle car = (new Vehicle(readline[0],
                        (VehicleGrade)Enum.Parse(typeof(VehicleGrade), readline[1].ToUpper()),
                        readline[2],
                        readline[3],
                        int.Parse(readline[4]),
                        int.Parse(readline[5]),
                        (TransmissionType)Enum.Parse(typeof(TransmissionType), readline[6].ToUpper()),
                        (FuelType)Enum.Parse(typeof(FuelType), readline[7].ToUpper()),
                        bool.Parse(readline[8]),
                        bool.Parse(readline[9]),
                        double.Parse(readline[10]),
                        readline[11]));

                    //add the vehicle to the list
                    vehicles.Add(car);
                }
                index++; //iterate the counter
            }

            //close the reader and inFile
            reader.Close();
            inFile.Close();
        }
        public void LoadRentalsFromFile()
        {
            //clear rentals before loading them
            rentals.Clear();

            //open the file stream and stream reader
            FileStream inFile = new FileStream(rentalFile, FileMode.Open);
            StreamReader reader = new StreamReader(inFile);

            //counter to be used by the while loop
            int index = 0;

            //while it is not the end of the stream
            while (!reader.EndOfStream)
            {
                //reader the current line
                string thisLine = reader.ReadLine();

                //split the line into seperate parts by the ',' character
                string[] readline = thisLine.Split(',');

                //skip the first line as the first line is the headings of the attributes
                if (index != 0)
                {
                    //if not the first line add the rented vehicle and customer to the rentals list
                    rentals.Add(readline[0], int.Parse(readline[1]));
                }

                index++; //iterate the counter
            }

            //close the reader and inFile
            reader.Close();
            inFile.Close();
        }

        //this method asks user for what details theyre searching for and return a list of vehicles
        //containing those details
        public void SearchVehicle()
        {
            //ask user for details of vehicle they want to find
            Console.WriteLine("Please enter details of vehicles you want");
            //read the result
            string result = Console.ReadLine();

            //if the user entered nothing
            if (result == "" || string.IsNullOrWhiteSpace(result))
            {
                //print every vehicle
                foreach (Vehicle car in GetFleet(false))
                {
                    Console.WriteLine(car.ToString());
                }
            }

            //if the entry does not contain brackets then
            else if (!result.Contains("(") && !result.Contains(")"))
            {
                //counter to be used
                int counter = 0;

                //split the entry into or components
                string[] or = result.ToUpper().Split(new string[] { " OR " }, StringSplitOptions.None);

                //create new list to save vehicles whihc match the description
                List<Vehicle> result_list = new List<Vehicle>();

                //for each detail in or
                foreach (string word in or)
                {
                    //if the statement contains AND then
                    if (word.ToUpper().Contains(" AND "))
                    {
                        //split the word up into and components
                        string[] and = word.ToUpper().Split(new string[] { " AND " }, StringSplitOptions.None);

                        //check each vehicle in the fleet
                        foreach (Vehicle car in GetFleet(false))
                        {
                            //initialise the counter
                            counter = 0;

                            //for each word in the string list "AND"
                            foreach (string word2 in and)
                            {
                                //if the car contains the detail
                                if (car.FullDescription().ToUpper().Contains(word2.ToUpper()))
                                {
                                    //iterate the counter
                                    counter++;
                                }
                            }
                            //if the counter iterates at every "and" statement, it will equal the number of statements in "and"
                            if (counter == and.Length)
                            {
                                //if the result list doesnt currently contain this particular car
                                if (!result_list.Contains(car))
                                {
                                    //add the car
                                    result_list.Add(car);
                                }
                            }
                        }
                    }
                    //if the component of "or" does not contain "and"
                    else
                    {
                        //foreach vehicle in fleet
                        foreach (Vehicle car in GetFleet(false))
                        {
                            //if the vehicle contains the componenet
                            if (car.FullDescription().ToUpper().Contains(word.ToUpper()))
                            {
                                //if the result list does not already contain the vehicle
                                if (!result_list.Contains(car))
                                {
                                    //add the vehicle to the result list
                                    result_list.Add(car);
                                }
                            }
                        }
                    }
                }
                //if the result list is empty
                if (result_list.Count == 0)
                {
                    Console.WriteLine("No vehicles match your entry");
                }
                //if result is not empty print all the vehicles in the list
                else
                {
                    //print the attribute headers
                    Console.WriteLine("Registration".PadRight(14) +
                                "Grade".PadRight(15) +
                                "Make".PadRight(12) +
                                "Model".PadRight(13) +
                                "Year".PadRight(6) +
                                "NumSeats".PadRight(9) +
                                "Transmission".PadRight(13) +
                                "Fuel".PadRight(7) +
                                "GPS".PadRight(7) +
                                "SunRoof".PadRight(8) +
                                "DailyRate".PadRight(10) +
                                "Colour".PadRight(5));
                    //print all the vehicles in the list
                    foreach (Vehicle car in result_list)
                    {
                        Console.WriteLine(car.ToString());
                    }
                }
            }

            //if entry contains brackets
            else
            {
                //turn the entry into infix notation
                List<Vehicle_Token> inFix = ParseText(result);
                
                //turn the infix notation into postfix notation
                List<Vehicle_Token> PostFix = Shunting_Yard(inFix);

                //evaluate the PostFix notation to a final list
                List<Vehicle> final_list = Evaluate(PostFix);

                //if the final list is empty
                if (final_list.Count == 0)
                {
                    Console.WriteLine("No vehicles match your entry");
                }
                //if the list is not empty
                else
                {
                    //print the attribute list
                    Console.WriteLine("Registration".PadRight(14) +
                                "Grade".PadRight(15) +
                                "Make".PadRight(12) +
                                "Model".PadRight(13) +
                                "Year".PadRight(6) +
                                "NumSeats".PadRight(9) +
                                "Transmission".PadRight(13) +
                                "Fuel".PadRight(7) +
                                "GPS".PadRight(7) +
                                "SunRoof".PadRight(8) +
                                "DailyRate".PadRight(10) +
                                "Colour".PadRight(5));

                    //print all the vehicles in the fina list
                    foreach (Vehicle car in final_list)
                    {
                        Console.WriteLine($"{car.ToString()}");
                    }
                }
            }
        }

        //this method takes a string and turns it into a lsit of tokens in inFix notation
        //which can be easily operated on
        public static List<Vehicle_Token> ParseText(string input)
        {
            //variables to be used
            string result = "";
            int counter = 0;
            bool quotes = false;

            //create a new list for the output
            List<Vehicle_Token> output = new List<Vehicle_Token>();

            int right_bracket = 0;
            int left_bracket = 0;

            foreach (char character in input)
            {
                if (character == '(')
                {
                    left_bracket++;
                }
                else if (character == ')')
                {
                    right_bracket++;
                }
            }

            //check each characeter in the input
            foreach (char character in input)
            {
                //if character is a left bracket then add a bracket token
                if (character == '(')
                {
                    output.Add(new LeftBracket_Token());
                }
                //if character is a right bracket
                else if (character == ')')
                {
                    //if the result has values in it
                    if (counter != 0)
                    {
                        //add the detail
                        output.Add(new Detail_Token(result));

                        //clear the result and counter
                        result = "";
                        counter = 0;
                    }
                    //add a right bracket token
                    output.Add(new RightBracket_Token());
                }
                //if character is a quotation amrk
                else if (character == '"')
                {
                    //flip the current qoutes boolean value
                    if (quotes == false)
                    {
                        quotes = true;
                    }
                    else if (quotes == true)
                    {
                        quotes = false;
                    }
                }
                //if character is a space
                else if (character == ' ')
                {
                    //if there is no quotes and there are values in result
                    if (quotes == false && counter != 0)
                    {
                        //if result is OR then add an or operator token
                        if (result.ToUpper() == "OR")
                        {
                            output.Add(new Operator_Token("or", 1));
                        }
                        //if result is AND then add an and operator token
                        else if (result.ToUpper() == "AND")
                        {
                            output.Add(new Operator_Token("and", 2));
                        }
                        //add a detail token
                        else
                        {
                            output.Add(new Detail_Token(result));
                        }
                        //clear the result and counter after they have been added
                        result = "";
                        counter = 0;
                    }
                    //if there is a quotation mark 
                    else if (quotes == true)
                    {
                        //add the space into the detail result
                        result += ' ';
                    }
                }
                //if the character isnt a bracket or quote or space
                else
                {
                    //add the character to the retail result
                    result += character;
                    counter++;
                }

            }
            //if there is information in the result string
            if (counter != 0)
            {
                //add the result to the list and clear it and counter
                output.Add(new Detail_Token(result));
                result = "";
                counter = 0;
            }

            //return the list
            return output;
        }

        //the Shunting_Yard algorythm takes a list of vehicles and sorts them in a way where they can be easily evaluated
        //and returns the sorted list
        public List<Vehicle_Token> Shunting_Yard(List<Vehicle_Token> input)
        {
            //create new PostFix notation list
            List<Vehicle_Token> PostFix = new List<Vehicle_Token>();

            //create new stack to operator the tokens on
            Stack<Vehicle_Token> stack = new Stack<Vehicle_Token>();

            //for every token inputed
            foreach (Vehicle_Token token in input)
            {
                //if the token is a detail token, add it to the final list
                if (token is Detail_Token)
                {
                    PostFix.Add(token);
                }
                //if the token is an operator token
                else if (token is Operator_Token)
                {
                    //while there is anything in the stack and the top of stack is an operator token
                    //and the priority of the operator on the top of the stack is greater than the current
                    //token priority
                    while (stack.Any() && stack.Peek() is Operator_Token
                        && ((Operator_Token)stack.Peek()).Priority >= ((Operator_Token)token).Priority)
                    {
                        //remove the top of the stack and add it to the list
                        PostFix.Add(stack.Pop());
                    }
                    //add the token to the top of the stack
                    stack.Push(token);
                }
                //if token is a left bracket then add it to the stack
                else if (token is LeftBracket_Token)
                {
                    stack.Push(token);
                }
                //if token is a right bracket then
                else if (token is RightBracket_Token)
                {
                    //while the top of stack is not a left bracket
                    while (!(stack.Peek() is LeftBracket_Token))
                    {
                        //pop elements from the stack and add it to the list
                        PostFix.Add(stack.Pop());
                    }
                    //remove the top element of the stack, which is the left bracket token
                    stack.Pop();
                }
            }
            //while there is anything in the stack
            while (stack.Any())
            {
                //remove item from stack and add it to the list
                PostFix.Add(stack.Pop());
            }

            //return the final list
            return PostFix;
        }

        //the evaluate function takes a list of postfix notation vehicle tokens and returns a list of vehicles
        //it reads each token in the inputed list and evaluates them then adds the according vehicles to the final list
        public List<Vehicle> Evaluate(List<Vehicle_Token> input)
        {
         
                //stack to be used to evaluate tokens
                Stack<Vehicle_Token> detail_stack = new Stack<Vehicle_Token>();

                //temporary lists to evaluate vehicles
                List<Vehicle> list_A = new List<Vehicle>();
                List<Vehicle> list_B = new List<Vehicle>();
                List<Vehicle> list_temporary = new List<Vehicle>();

                //list of all the vehicles in the fleet
                List<Vehicle> fleet_list = GetFleet(false);

                //temporary string variables to hold the details that the user is looking for
                string detail_A;
                string detail_B;

                //first list acts differently from every other list so check if it is the first list
                bool first_list = true;
                //counter to help check if it is the first list
                int counter = 0;

                //for every vehicle token in the input
                foreach (Vehicle_Token token in input)
                {
                    //if it is a vehicle token, add it to the stack
                    if (token is Detail_Token)
                    {
                        detail_stack.Push(token);
                    }
                    //if it is an operator token
                    else if (token is Operator_Token)
                    {
                        //if there are greater than or equal to 2 values in the list
                        if (detail_stack.Count >= 2)
                        {
                            //if it is not the first list then change value of first list to false
                            if (counter != 0)
                            {
                                first_list = false;
                            }

                            //if it is the first list
                            if (first_list == true)
                            {
                                //iterate the counter so it is not the first list anymore
                                counter++;

                                //if the operator token is "or"
                                if (token.ToString() == "or")
                                {
                                    //pop the two details in the stack and assign them to variables
                                    detail_A = detail_stack.Pop().ToString();
                                    detail_B = detail_stack.Pop().ToString();

                                    //for every vehicle in the fleet
                                    foreach (Vehicle car in fleet_list)
                                    {
                                        //if the cars description contains detail A or detail B and is not currently in the list
                                        if ((car.FullDescription().ToUpper().Contains(detail_A.ToUpper()) ||
                                            car.FullDescription().ToUpper().Contains(detail_B.ToUpper())) &&
                                            !list_A.Contains(car))
                                        {
                                            //add the car to the list A
                                            list_A.Add(car);
                                        }
                                    }
                                }

                                //if the operator token is "and"
                                else if (token.ToString() == "and")
                                {
                                    //pop the two items on the stack and assign then to varibales
                                    detail_A = detail_stack.Pop().ToString();
                                    detail_B = detail_stack.Pop().ToString();

                                    //check every vehicle in the fleet
                                    foreach (Vehicle car in fleet_list)
                                    {
                                        //if the vehicles description contains detail A and detail B and it does not already contain the vehicle
                                        if (car.FullDescription().ToUpper().Contains(detail_A.ToUpper()) &&
                                            car.FullDescription().ToUpper().Contains(detail_B.ToUpper()) &&
                                            !list_A.Contains(car))
                                        {
                                            //add vehicle to the list A
                                            list_A.Add(car);
                                        }
                                    }
                                }
                            }
                            //if it is not the first list evaluated
                            else if (first_list == false)
                            {
                                //if token is "or"
                                if (token.ToString() == "or")
                                {
                                    //pop two details on the stack and assign them to variables
                                    detail_A = detail_stack.Pop().ToString();
                                    detail_B = detail_stack.Pop().ToString();

                                    //check every car in the fleet
                                    foreach (Vehicle car in fleet_list)
                                    {
                                        //if the vehicels description contains detail A or detail B and does not exist in the lsit
                                        if ((car.FullDescription().ToUpper().Contains(detail_A.ToUpper()) ||
                                            car.FullDescription().ToUpper().Contains(detail_B.ToUpper())) &&
                                            !list_B.Contains(car))
                                        {
                                            //add vehicle to list B
                                            list_B.Add(car);
                                        }
                                    }
                                }
                                //if the token is "and"
                                else if (token.ToString() == "and")
                                {
                                    //pop the details on the stack and assign them to variables
                                    detail_A = detail_stack.Pop().ToString();
                                    detail_B = detail_stack.Pop().ToString();

                                    //check every vehicle in fleet
                                    foreach (Vehicle car in fleet_list)
                                    {
                                        //if the vheicles description contain detail A and detail B and does not current exist in the list
                                        if (car.FullDescription().ToUpper().Contains(detail_A.ToUpper()) &&
                                            car.FullDescription().ToUpper().Contains(detail_B.ToUpper()) &&
                                            !list_B.Contains(car))
                                        {
                                            //add vehicle to list B
                                            list_B.Add(car);
                                        }
                                    }
                                }
                            }
                        }
                        //if there is only one item in the stack
                        else if (detail_stack.Count == 1)
                        {
                            //if it is the first list
                            if (first_list == true)
                            {
                                //if token is "or"
                                if (token.ToString() == "or")
                                {
                                    //pop one value from the stack and assign it to a variable
                                    detail_A = detail_stack.Pop().ToString();

                                    //check every vehicle in the fleet
                                    foreach (Vehicle car in fleet_list)
                                    {
                                        //if the vehicels description contains detail A and does not currently exist in the fleet
                                        if (car.FullDescription().ToUpper().Contains(detail_A.ToUpper()) &&
                                            !list_A.Contains(car))
                                        {
                                            //add vehicle to list A
                                            list_A.Add(car);
                                        }
                                    }
                                }
                                //if token is "and:
                                else if (token.ToString() == "and")
                                {
                                    //pop one value from the stack and assign it to a variable
                                    detail_A = detail_stack.Pop().ToString();

                                    //check every vehicle in the fleet
                                    foreach (Vehicle car in list_A.ToList())
                                    {
                                        //if the vehicles description does not contain detail A
                                        if (!car.FullDescription().ToUpper().Contains(detail_A.ToUpper()))
                                        {
                                            //remove vehicle from list A
                                            list_A.Remove(car);
                                        }
                                    }
                                }
                            }
                            //if it is not the first list
                            else if (first_list == false)
                            {
                                //If list B is empty
                                if (list_B.Count == 0)
                                {
                                    //if the token is "or"
                                    if (token.ToString() == "or")
                                    {
                                        //pop the detail from the stack and assign it to a varible
                                        detail_A = detail_stack.Pop().ToString();

                                        //for every vehicle in the fleet
                                        foreach (Vehicle car in fleet_list)
                                        {
                                            //if the vehicle description cotnains detail A and is not currently in the lsit
                                            if (car.FullDescription().ToUpper().Contains(detail_A.ToUpper()) &&
                                                !list_A.Contains(car))
                                            {
                                                //add vehicle to lsit A
                                                list_A.Add(car);
                                            }
                                        }
                                    }
                                    //if the token is "and"
                                    else if (token.ToString() == "and")
                                    {
                                        //pop the detail from the stack and assign it to a variable
                                        detail_A = detail_stack.Pop().ToString();

                                        //check every vehicle in List A
                                        foreach (Vehicle car in list_A.ToList())
                                        {
                                            //if the vehicle in list A does not contain the detail A
                                            if (!car.FullDescription().ToUpper().Contains(detail_A.ToUpper()))
                                            {
                                                //remove vehicle from list A
                                                list_A.Remove(car);
                                            }
                                        }
                                    }
                                }
                                //if there is more than zero items in list B
                                else
                                {
                                    //if token is "or"
                                    if (token.ToString() == "or")
                                    {
                                        //pop the detail on stack and assign it to a variable
                                        detail_A = detail_stack.Pop().ToString();

                                        //check every vehicle in the fleet
                                        foreach (Vehicle car in fleet_list)
                                        {
                                            //if vehicle description contain detail A and is not currently in list B
                                            if (car.FullDescription().ToUpper().Contains(detail_A.ToUpper()) &&
                                                !list_B.Contains(car))
                                            {
                                                //add vehicle to list B
                                                list_B.Add(car);
                                            }
                                        }
                                    }
                                    //if token is "and"
                                    else if (token.ToString() == "and")
                                    {
                                        //pop item from stack and assign it to variable
                                        detail_A = detail_stack.Pop().ToString();

                                        //check every vehicle in List B
                                        foreach (Vehicle car in list_B)
                                        {
                                            //if vehicle description does not contain detail A
                                            if (!car.FullDescription().ToUpper().Contains(detail_A.ToUpper()))
                                            {
                                                //remove vehicle from list B
                                                list_B.Remove(car);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //if there are no items in the stack
                        else if (detail_stack.Count == 0)
                        {
                            //if the token is "or"
                            if (token.ToString() == "or")
                            {
                                //check every vehicle in list B
                                foreach (Vehicle car in list_B)
                                {
                                    //if list A does not contain the vehicle
                                    if (!list_A.Contains(car))
                                    {
                                        //add vehicle from list B to list A
                                        list_A.Add(car);
                                    }
                                }
                            }
                            //if token is "and"
                            else if (token.ToString() == "and")
                            {
                                //check every vehicle in list B
                                foreach (Vehicle car in list_B)
                                {
                                    //if list A contains vehicle
                                    if (list_A.Contains(car))
                                    {
                                        //add vehicle to a temporary list
                                        list_temporary.Add(car);
                                    }
                                }
                                //clear list
                                list_A.Clear();
                                //add all vehicles in the temporary list to list A
                                foreach (Vehicle car in list_temporary)
                                {
                                    list_A.Add(car);
                                }
                            }

                            //clear temporary list and list B
                            list_temporary.Clear();
                            list_B.Clear();
                        }
                    }
                }

                //return list A
                return list_A;
           
        }         
    }
}
