using System.Runtime.Serialization.Formatters;

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
        public string Entrance { set; get; }

        public Address(string city,string street="" ,string building="",string entrance="")
        {
            City = city;
            Building = building;
            Street = street;
            Entrance= entrance;
        }

        public override string ToString()
        {
            var str = City;
            if(Building!="")
                str += ", " + Building;
            if (Street != "")
                str += ", " + Street;
            if (Entrance != "")
                str += ", " + Entrance;
            return str;
        }
    }
}
