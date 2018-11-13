using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Represent a address
    /// </summary>
    public class Address
    {
        public string City { set; get; }
        public string Street { set; get; }
        public string Building { set;get; }
        public string Enterence { set; get; }

        public Address(string city=null,string street=null ,string building=null,string enterence=null)
        {
            City = city;
            Building = building;
            Street = street;
            enterence = Enterence;
        }

        public override string ToString()
        {
            string str=null;
            if(City!=null&&Street!=null&&Building!=null)
                str = City + ", " + Street + ", " + Building;
            if (Enterence != null)
                str += Enterence;
            return str;
        }
    }
}
