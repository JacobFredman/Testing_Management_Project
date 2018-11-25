using System;

namespace BE
{
    public class Route
    {
        public string[] addresses { private set; get; }
        public string Start { get { return addresses[0]; } private set { } }
        public string End { get { return addresses[addresses.Length - 1]; } private set { } }
        public Route(params string[] addresses)
        {
            this.addresses = new string[addresses.Length - 2];
            if (addresses.Length < 2)
                throw new Exception("This is not a route");
            this.addresses = addresses;
        }
        public string GetGoogleURL()
        {
            string url = "https://www.google.com/maps/dir/?api=1&origin=" + Start + "&destination="
                + End + "&travelmode=driving" + "&waypoints=";
            for (int i = 1; i < addresses.Length - 1; i++)
            {
                url += addresses[i] + "|";
            }
            url = url.TrimEnd('|');
            return url;
        }

    }
}
