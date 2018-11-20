using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BE
{
    public class Tools
    {
        /// <summary>
        /// Check if Israely ID is valied
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>true if it is valied</returns>
        public static bool CheckID_IL(uint id)
        {
            uint[] idArr = new uint[9];
            for (int i = 8; i >= 0; i--)
            {
                idArr[i] = id % 10;
                id /= 10;
            }
            for (uint i = 0; i < 9; i++)
            {
                idArr[i] *= i % 2 + 1;
            }
            for (uint i = 0; i < 9; i++)
            {
                idArr[i] = idArr[i] / 10 + idArr[i] % 10;
            }
            uint sum = 0;
            for (uint i = 0; i < 9; i++)
            {
                sum += idArr[i];
            }
            if (sum % 10 != 0)
                return false;
            return true;
        }

        /// <summary>
        /// returns the distance between to points from google maps
        /// </summary>
        /// <param name="origin">an address</param>
        /// <param name="destination">an addres</param>
        /// <returns>the distance in meters</returns>
        public static int GetDistanceGoogleMapsAPI(Address origin, Address destination)
        {
            int distance = 0;
            string content="";
            string requesturl = Configuration.GoogleDistanceURL + "json?" + "key="+ Configuration.Key 
                + "&origin=" + origin.ToString() + "&destination=" + destination.ToString() + "&sensor=false";
           if (requesturl.ToLower().IndexOf("https:") > -1|| requesturl.ToLower().IndexOf("http:") > -1)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(requesturl);
                content = System.Text.Encoding.ASCII.GetString(response);
                JObject o = JObject.Parse(content);
                distance = (int)o.SelectToken("routes[0].legs[0].distance.value");
                return distance;
            }
            else
                throw new Exception("Google URL is not correct");  
        }
 
        /// <summary>
        /// returns the travel time between to points from google maps
        /// </summary>
        /// <param name="origin">an address</param>
        /// <param name="destination">an addres</param>
        /// <returns>the distance in meters</returns>
        public static int GetTravelTimeGoogleMapsAPI(Address origin, Address destination)
        {
            int distance = 0;
            string content = "";
            string requesturl = Configuration.GoogleDistanceURL+ "json?" + "key=" + Configuration.Key + "&origin=" + origin.ToString() + "&destination=" + destination.ToString() + "&sensor=false";
            if (requesturl.ToLower().IndexOf("https:") > -1 || requesturl.ToLower().IndexOf("http:") > -1)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(requesturl);
                content = System.Text.Encoding.ASCII.GetString(response);
                JObject o = JObject.Parse(content);
                distance = (int)o.SelectToken("routes[0].legs[0].duration.value");
                return distance;
            }
            else
                throw new Exception("Google URL is not correct");
        }

        /// <summary>
        /// returns the distance between to points from google maps
        /// </summary>
        /// <param name="origin">an address</param>
        /// <param name="destination">an addres</param>
        /// <returns>the distance in meters</returns>
        public static int GetDistanceGoogleMapsAPIXML(Address origin, Address destination)
        {
            int distance = 0;
            string content = "";
            string requesturl = Configuration.GoogleDistanceURL+ "xml?" + "key=" + Configuration.Key + "&origin=" + origin.ToString() + "&destination=" + destination.ToString() + "&sensor=false";
            if (requesturl.ToLower().IndexOf("https:") > -1 || requesturl.ToLower().IndexOf("http:") > -1)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(requesturl);
                content = System.Text.Encoding.ASCII.GetString(response);
                XElement xml = XElement.Parse(content);
                distance = int.Parse(xml.Element("route").Element("leg").Element("distance").Element("value").Value);
                return distance;
            }
            else
                throw new Exception("Google URL is not correct");
        }

        /// <summary>
        /// returns the travel time between to points from google maps
        /// </summary>
        /// <param name="origin">an address</param>
        /// <param name="destination">an addres</param>
        /// <returns>the distance in meters</returns>
        public static int GetTravelTimeGoogleMapsAPIXML(Address origin, Address destination)
        {
            int distance = 0;
            string content = "";
            string requesturl = Configuration.GoogleDistanceURL + "xml?" + "key=" + Configuration.Key + "&origin=" + origin.ToString() + "&destination=" + destination.ToString() + "&sensor=false";
            if (requesturl.ToLower().IndexOf("https:") > -1 || requesturl.ToLower().IndexOf("http:") > -1)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(requesturl);
                content = System.Text.Encoding.ASCII.GetString(response);
                XElement xml = XElement.Parse(content);
                distance = int.Parse(xml.Element("route").Element("leg").Element("duration").Element("value").Value);
                return distance;
            }
            else
                throw new Exception("Google URL is not correct");
        }

        /// <summary>
        /// get a string from an exeption
        /// </summary>
        /// <param name="ex">Exseption</param>
        /// <param name="Source">include the scoure too</param>
        /// <param name="Length">max length</param>
        /// <returns></returns>
        public static string GetExpectionMEssage(Exception ex, bool Source = true, int Length = 100)
        {
            Length -= 22;
            Length -= ex.Message.Length;
            string str = "ERROR: " + ex.Message + "!!" + (Source ? ("\tSource:" + ex.StackTrace) : "");
            return ((str.Length > Length) ? (str.Substring(0, Length) + "...") : str);
        }

        public static string GetMapWayPointURL(Address source,Address destination,Address[] wayPionts)
        {
            string url = "https://www.google.com/maps/dir/?api=1&origin=" + source.ToString() + "&destination="
                + destination.ToString() + "&travelmode=driving" + "&waypoints=";
            foreach(Address ad in wayPionts)
            {
                url += ad.ToString() + "|";
            }
            url = url.TrimEnd('|');
            return url;
        }

    }
}
