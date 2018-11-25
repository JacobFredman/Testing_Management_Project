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

        public Address(string city="",string street="" ,string building="",string enterence="")
        {
            City = city;
            Building = building;
            Street = street;
            enterence = Enterence;
        }

        public override string ToString()
        {
            string str="";
            if(City!=""&&Street!=""&&Building!="")
                str = City + ", " + Street + ", " + Building;
            if (Enterence != "")
                str +=", "+ Enterence;
            return str;
        }
    }
}
