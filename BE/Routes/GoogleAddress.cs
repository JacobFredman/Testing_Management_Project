namespace BE.Routes
{
    /// <summary>
    /// Google address with name and id
    /// </summary>
    public class GoogleAddress
    {
        /// <summary>
        /// Google address
        /// </summary>
        /// <param name="name">Address description</param>
        /// <param name="id">Address place_id</param>
        public GoogleAddress(string name = "", string id = "")
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Address description
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Address place_id
        /// </summary>
        public string Id { set; get; }
    }
}