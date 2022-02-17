using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    class LeftBracket_Token : Vehicle_Token
    {
        //returns a left bracket character
        public override string ToString()
        {
            return "(";
        }
    }
}
