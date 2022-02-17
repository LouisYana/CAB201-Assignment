using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    class Detail_Token : Vehicle_Token
    {
        //variable to be used by detail token
        public string detail;

        //creates new detail token with name
        public Detail_Token(string detail)
        {
            this.detail = detail;
        }

        //returns the detail tokens name in string format
        public override string ToString()
        {
            return detail.ToString();
        }
    }
}
