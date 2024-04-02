using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public class Enums
    {
        public enum OrderType // you should rename this field also in DB
        {
            File = 1,
            Manual = 2
        }

        public enum OrderStatus // if you want a new table in DB and change status (from Orders table) in int, you can than make them ints too 
        {
            Initialized = 1,
            Processed = 3,
        }
    }
}
