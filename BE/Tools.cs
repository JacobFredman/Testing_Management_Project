using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="origin">an addressLatLog</param>
        /// <param name="destination">an addres</param>
        /// <returns>the distance in meters</returns>
        public static int GetDistanceGoogleMapsAPI(Address origin, Address destination)
        {
            int distance = 0;
            string content="";
            string requesturl = Configuration.GoogleDistanceURL + "key="+ Configuration.Key 
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
        /// <param name="origin">an addressLatLog</param>
        /// <param name="destination">an addres</param>
        /// <returns>the distance in meters</returns>
        public static int GetTravelTimeGoogleMapsAPI(Address origin, Address destination)
        {
            int distance = 0;
            string content = "";
            string requesturl = Configuration.GoogleDistanceURL + "key=" + Configuration.Key + "&origin=" + origin.ToString() + "&destination=" + destination.ToString() + "&sensor=false";
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

        

    }
}
