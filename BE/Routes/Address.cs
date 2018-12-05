namespace BE.Routes
{
    /// <summary>
    /// Represents an address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// The city or the whole address
        /// </summary>
        public string City { set; get; }
        /// <summary>
        /// The street
        /// </summary>
        public string Street { set; get; }
        /// <summary>
        /// The building
        /// </summary>
        public string Building { set;get; }
        /// <summary>
        /// The Building entrance
        /// </summary>
        public string Entrance { set; get; }

        /// <summary>
        /// A new AddressOfBeginningTest Must include at least one string
        /// </summary>
        /// <param name="city">City</param>
        /// <param name="street">Street</param>
        /// <param name="building">Building</param>
        /// <param name="entrance">Entrance</param>
        public Address(string city,string street="" ,string building="",string entrance="")
        {
            City = city;
            Building = building;
            Street = street;
            Entrance= entrance;
        }

        /// <summary>
        /// Get the full address
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = City;
            if (Street != "")
                str += ", " + Street;
            if(Building!="")
                str += ", " + Building;
            if (Entrance != "")
                str += ", " + Entrance;
            return str;
        }
    }
}
