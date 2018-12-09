using Newtonsoft.Json.Linq;
using System;
using BE.Routes;

namespace BE
{
    public class Tools
    {
        /// <summary>
        /// Check if Israely Id is valid
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>true if it is valid</returns>
        public static bool CheckID_IL(uint Id)
        {
            if (Id == 0) return false;
            uint[] idArr = new uint[9];

            //put the Id in an arr
            for (int i = 8; i >= 0; i--)
            {
                idArr[i] = Id % 10;
                Id /= 10;
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

            //check the Id
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
            var request = Configuration.GoogleDistanceUrl + "key="+ Configuration.Key 
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
