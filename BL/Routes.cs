using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using BE;
using BE.MainObjects;
using BE.Routes;
using Newtonsoft.Json.Linq;

namespace BL
{
    public static class Routes
    {
        /// <summary>
        ///     Open a link in new chrome window
        /// </summary>
        /// <param name="url"></param>
        public static void ShowUrlInChromeWindow(Uri url)
        {
            Process.Start("chrome.exe", "--app=" + url.AbsoluteUri);
        }

        /// <summary>
        ///     Find a route for the test and set the address of the test
        /// </summary>
        /// <param name="test">the test</param>
        /// <param name="address">the address</param>
        public static void SetRouteAndAddressToTest(this Test test, Address address)
        {
            try
            {
                //get locations around the address in the default radios
                var arr = GetLocationsInRadius(GetLocationLatLog(new Address(address.ToString())))
                    .Distinct().ToList();

                //shrink the list 
                arr = arr.Skip(1).Take(5).ToList();
                arr.Insert(0, GetAddressSuggestionsGoogle(test.AddressOfBeginningTest.ToString(), "0").First());

                //get the duration of the route
                var duration = GetRouteDuration(arr.ToArray());

                //if the route takes too much time then find a new route where the radios is 500m shorter 
                if (duration > Configuration.MaxTestDurationSec && arr.Count > 4)
                {
                    arr = GetLocationsInRadius(GetLocationLatLog(new Address(address.ToString())),
                            (uint) (Configuration.MaxTestDurationSec - 500))
                        .Distinct().Skip(1).Take(6).ToList();
                    duration = GetRouteDuration(arr.ToArray());
                }

                //if the route is too short then find a route where the radios is 500m longer
                if (duration < Configuration.MinTestDurationSec && arr.Count > 4)
                {
                    arr = GetLocationsInRadius(GetLocationLatLog(new Address(address.ToString())),
                            (uint) (Configuration.MinTestDurationSec + 500))
                        .Distinct().Skip(1).Take(7).ToList();
                    duration = GetRouteDuration(arr.ToArray());
                }

                //if the duration of the route is still not ok then throw exception
                if (duration < Configuration.MinTestDurationSec || duration > Configuration.MaxTestDurationSec)
                    throw new GoogleAddressException("Can't find a route near the given address", "NO_ROUTE");

                //create an url to show thw route on a map
                test.RouteUrl = new Uri(GetGoogleUrl(arr.ToArray()));
            }
            catch (Exception ex)
            {
                //if there was an error
                test.RouteUrl = null;
                //test.AddressOfBeginningTest = null;
                //check that it throw an GoogleAddressException  
                if (!(ex is GoogleAddressException gex))
                    throw new GoogleAddressException(ex.Message, "CONNECTION_FAILURE");
                throw new GoogleAddressException(ex.Message + gex.ErrorCode, "ADDRESS_FAILURE");
            }
        }

        /// <summary>
        ///     Get suggestions for places from google
        /// </summary>
        /// <param name="input">the input to search</param>
        /// <param name="token">a session token</param>
        /// <returns></returns>
        public static IEnumerable<GoogleAddress> GetAddressSuggestionsGoogle(string input, string token)
        {
            var url = "https://maps.googleapis.com/maps/api/place/autocomplete/xml?input=" + input +
                      "&types=address&components=country:il&language=iw&key=" + Configuration.Key + "&sessiontoken=" +
                      token;
            try
            {
                var xml = DownloadDataIntoXml(url);
                return (from adr in xml.Elements()
                        where adr.Name == "prediction"
                        select new GoogleAddress(adr.Element("description").Value, adr.Element("place_id").Value)
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     returns the distance between to points from google maps
        /// </summary>
        /// <param name="origin">an addressLatLog</param>
        /// <param name="destination">an address</param>
        /// <returns>the distance in meters</returns>
        public static int GetDistanceGoogleMapsApi(Address origin, Address destination, int times = 0)
        {
            try
            {
                //create the url
                var request = Configuration.GoogleDistanceUrl + "json?" + "key=" + Configuration.Key
                              + "&origin=" + origin + "&destination=" + destination + "&sensor=false";

                //check the url
                if (request.ToLower().IndexOf("https:", StringComparison.Ordinal) <= -1 &&
                    request.ToLower().IndexOf("http:", StringComparison.Ordinal) <= -1)
                    throw new Exception("Google URL is not correct");

                //download the data
                var wc = new WebClient();
                var response = wc.DownloadData(request);
                var contentResponse = Encoding.UTF8.GetString(response);
                //parse it json
                var jsonResponse = JObject.Parse(contentResponse);
                var distance = (int) jsonResponse.SelectToken("routes[0].legs[0].distance.value");

                return distance;
            }
            catch (Exception ex)
            {
                if (times == 2)
                    throw new Exception(ex.Message);
                Thread.Sleep(2000);
                return GetDistanceGoogleMapsApi(origin, destination, ++times);
            }
        }


        #region Help Functions

        /// <summary>
        ///     Get an array of locations in the radios of the location
        /// </summary>
        /// <param name="locationLatLog">the location in lat,log for example 31.750068,34.9907657 </param>
        /// <param name="radios">the radios in meters</param>
        /// <returns>an array of address with name and Id</returns>
        private static IEnumerable<GoogleAddress> GetLocationsInRadius(string locationLatLog, uint radios = 2000)
        {
            //create the url
            var url = "https://maps.googleapis.com/maps/api/place/nearbysearch/xml?key=" + Configuration.Key +
                      "&location=" + locationLatLog + "&radius=" + radios + " & language=wi";

            //download the data
            var xml = DownloadDataIntoXml(url);

            //get all the results
            return (from adr in xml.Elements()
                where adr.Name == "result" && adr.Element("vicinity")?.Value.ToLower() != "israel"
                select new GoogleAddress
                {
                    Name = adr.Element("vicinity")?.Value + ", " + adr.Element("name")?.Value,
                    Id = adr.Element("place_id")?.Value
                }).ToArray();
        }

        /// <summary>
        ///     get the lat log location from an address
        /// </summary>
        /// <param name="address">the address</param>
        /// <returns>the location in lat,log for example 31.750068,34.9907657</returns>
        private static string GetLocationLatLog(Address address)
        {
            //create the url
            var url = "https://maps.googleapis.com/maps/api/place/textsearch/xml?key=" + Configuration.Key +
                      "&query=" + address;

            //download the data
            var xml = DownloadDataIntoXml(url);

            //return the value
            return xml.Element("result")?.Element("geometry")?.Element("location")?.Element("lat")?.Value + "," +
                   xml.Element("result")?.Element("geometry")?.Element("location")?.Element("lng")?.Value;
        }

        /// <summary>
        ///     get the time in sec of an route
        /// </summary>
        /// <param name="arr">the address</param>
        /// <returns>the route time in sec</returns>
        private static int GetRouteDuration(IReadOnlyList<GoogleAddress> arr)
        {
            //create the url of the start and end point
            var url = "https://maps.googleapis.com/maps/api/directions/xml?key=" + Configuration.Key +
                      "&origin=" + arr[0].Name + "&origin_place_id=" + arr[0].Id +
                      " &destination=" + arr[arr.Count - 1].Name + "&destination_place_id=" + arr[arr.Count - 1].Id +
                      " &waypoints=";

            //add the wayPoints
            for (var i = 1; i < arr.Count - 1; i++)
                url += arr[i].Name + "|";
            url = url.TrimEnd('|');

            //add the wayPoints Id
            url += "&waypoint_place_ids=";
            for (var i = 1; i < arr.Count - 1; i++) url += arr[i].Id + "|";
            url = url.TrimEnd('|');

            //download the data
            var xml = DownloadDataIntoXml(url);

            //return the sum of the durations
            return (from leg in xml.Elements("route").Elements()
                where leg.Name == "leg"
                select int.Parse(leg.Element("duration")?.Element("value")?.Value ??
                                 throw new InvalidOperationException())).Sum();
        }

        /// <summary>
        ///     Get the url to show the route on google maps
        /// </summary>
        /// <param name="arr">the address</param>
        /// <returns>an url</returns>
        private static string GetGoogleUrl(IReadOnlyList<GoogleAddress> arr)
        {
            //create the url for start point and end point
            var url = "https://www.google.com/maps/dir/?api=1" + "&travelmode=driving" +
                      "&origin=" + arr[0].Name + "&origin_place_id=" + arr[0].Id +
                      "&destination=" + arr[0].Name + "&destination_place_id=" + arr[0].Id +
                      "&waypoints=";

            //add the wayPoints
            for (var i = 1; i < arr.Count; i++) url += arr[i].Name + "|";
            url = url.TrimEnd('|');

            //add the wayPoints Ids
            url += "&waypoint_place_ids=";
            for (var i = 1; i < arr.Count; i++) url += arr[i].Id + "|";
            url = url.TrimEnd('|');

            return url;
        }

        /// <summary>
        ///     download the data from the url address into an xml
        ///     and check if the google api response was OK
        /// </summary>
        /// <param name="url">the url</param>
        /// <returns>the xml</returns>
        private static XElement DownloadDataIntoXml(string url)
        {
            //check the url
            if (url.ToLower().IndexOf("https:", StringComparison.Ordinal) <= -1 &&
                url.ToLower().IndexOf("http:", StringComparison.Ordinal) <= -1)
                throw new GoogleAddressException("Google URL is not correct", "WRONG_URL");

            //download the data into an xml
            var wc = new WebClient();
            var response = wc.DownloadData(url);
            var content = Encoding.UTF8.GetString(response);
            var xml = XElement.Parse(content);

            //check the request state
            if (xml.Element("status")?.Value != "OK")
                throw new GoogleAddressException("Google returns the next error: ", xml.Element("status")?.Value);
            return xml;
        }

        #endregion
    }
}