namespace BE.Routes
{
    public class GoogleAddress
    {
        public GoogleAddress(string name = "", string id = "")
        {
            Name = name;
            Id = id;
        }

        public string Name { set; get; }
        public string Id { set; get; }
    }
}