using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    
    class Operator_Token : Vehicle_Token
    {
        //variables of the operator token
        public string name;
        public int Priority { get; }

        //creates new operator token
        public Operator_Token(string name, int priority)
            {
                //assing the operator token its name and priority
                this.name = name;
                this.Priority = priority;
            }
        //returns the name of the operator token
        public override string ToString()
        {
            return name;
        }
    }
    
}
