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
        /// Check if Israely ID is valid
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>true if it is valid</returns>
        public static bool CheckID_IL(uint id)
        {
            uint[] idArr = new uint[9];

            //put the id in an arr
            for (int i = 8; i >= 0; i--)
            {
                idArr[i] = id % 10;
                id /= 10;
            }

            //multiply the odd digits and add one
            for (uint i = 0; i < 9; i++)
            {
                idArr[i] *= i % 2 + 1;
            }
            
            //sum the digits of the numbers
            for (uint i = 0; i < 9; i++)
            {
                idArr[i] = idArr[i] / 10 + idArr[i] % 10;
            }

            //Sum all the numbers
            uint sum = 0;
            for (uint i = 0; i < 9; i++)
            {
                sum += idArr[i];
            }

            //check the id
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
        public static int GetDistanceGoogleMapsApi(Address origin, Address destination)
        {
            //create the url
            var request = Configuration.GoogleDistanceURL + "key="+ Configuration.Key 
                + "&origin=" + origin.ToString() + "&destination=" + destination.ToString() + "&sensor=false";
            //check it
           if (request.ToLower().IndexOf("https:") > -1|| request.ToLower().IndexOf("http:") > -1)
            {
                //download the data
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(request);
                var content = System.Text.Encoding.ASCII.GetString(response);
                //parse it
                var o = JObject.Parse(content);
                var distance = (int)o.SelectToken("routes[0].legs[0].distance.value");
                //return it
                return distance;
            }
            else
                throw new Exception("Google URL is not correct");  
        }
     }
}
