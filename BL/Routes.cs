using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using BE;

namespace BL
{
    public static class Routes
    {
        public static void ShowUrlInChromeWindow(Uri url)
        {
            Process.Start("chrome.exe", "--app=" + url.AbsoluteUri);
        }

        /// <summary>
        /// Find a route for the test and set the address of the test
        /// </summary>
        /// <param name="test">the test</param>
        /// <param name="address">the address</param>
        public static void SetRouteAndAddressToTest(this Test test, Address address)
        {
            try
            {
                //get locations around the address in the default radios
                var arr = GetLocationsInRadios(GetLocationLatLog(new Address(address.ToString())))
                    .Distinct().ToArray();
                //shrink the list 
                arr = arr.Skip(1).Take(6).ToArray();
                //get the duration of the route
                var duration = GetRouteDuration(arr);

                //if the route takes too much time then find a new route where the radios is 500m shorter 
                if (duration > Configuration.MaxTestDurationSec && arr.Length > 4)
                {
                    arr = GetLocationsInRadios(GetLocationLatLog(new Address(address.ToString())),
                            (uint) (Configuration.MaxTestDurationSec - 500))
                        .Distinct().Skip(1).Take(6).ToArray();
                    duration = GetRouteDuration(arr);
                }

                //if the route is too short then find a route where the radios is 500m longer
                if (duration < Configuration.MinTestDurationSec && arr.Length > 4)
                {
                    arr = GetLocationsInRadios(GetLocationLatLog(new Address(address.ToString())),
                            (uint) (Configuration.MinTestDurationSec + 500))
                        .Distinct().Skip(1).Take(7).ToArray();
                    duration = GetRouteDuration(arr);
                }

                //if the duration of the route is still not ok then throw exception
                if (duration < Configuration.MinTestDurationSec || duration > Configuration.MaxTestDurationSec)
                    throw new GoogleAddressException("Can't find a route near the given address", "NO_ROUTE");
                //create an url to show thw route on a map
                test.RouteUrl = new Uri(GetGoogleUrl(arr));

                test.AddressOfBeginningTest = new Address(arr[0].Name);
            }
            catch (Exception ex)
            {
                //if there was an error
                test.RouteUrl = null;
                test.AddressOfBeginningTest = null;
                //check that it throw an GoogleAddressException  
                GoogleAddressException gex = ex as GoogleAddressException;
                if (gex == null)
                    throw new GoogleAddressException(ex.Message, "CONNECTION_FAILURE");
                throw new GoogleAddressException(ex.Message + gex.ErrorCode, "ADDRESS_FAILURE");
            }
        }

        #region Help Functions

        /// <summary>
        /// Get a arry of locations in the radios of the location
        /// </summary>
        /// <param name="locationLatLog">the location in lat,log for example 31.750068,34.9907657 </param>
        /// <param name="radios">the radios in meters</param>
        /// <returns>an arry of address with name and id</returns>
        private static GoogleAddress[] GetLocationsInRadios(string locationLatLog, uint radios = 2000)
        {
            //make the url
            var url = "https://maps.googleapis.com/maps/api/place/nearbysearch/xml?key=" + Configuration.Key +
                      "&location=" + locationLatLog + "&radius=" + radios + " & language=wi";

            //download the data
            XElement xml = DownloadDataIntoXml(url);

            //get all the results
            return ((from adr in xml.Elements()
                     where adr.Name == "result" && adr.Element("vicinity").Value.ToLower() != "israel"
                     select new GoogleAddress()
                     {
                         Name = adr.Element("vicinity").Value + ", " + adr.Element("name").Value,
                         Id = adr.Element("place_id").Value
                     }).ToArray());
        }

        /// <summary>
        /// get the lat log location from an address
        /// </summary>
        /// <param name="address">the address</param>
        /// <returns>the location in lat,log for example 31.750068,34.9907657</returns>
        private static string GetLocationLatLog(Address address)
        {
            //create the url
            var url = "https://maps.googleapis.com/maps/api/place/textsearch/xml?key=" + Configuration.Key +
                      "&query=" + address;

            //download the data
            XElement xml = DownloadDataIntoXml(url);

            //return the value
            return xml.Element("result").Element("geometry").Element("location").Element("lat").Value + "," +
                   xml.Element("result").Element("geometry").Element("location").Element("lng").Value;
        }

        /// <summary>
        /// get the time in sec of an route
        /// </summary>
        /// <param name="arr">the address</param>
        /// <returns>the time in sec</returns>
        private static int GetRouteDuration(GoogleAddress[] arr)
        {
            //create the url of the start and end point
            var url = "https://maps.googleapis.com/maps/api/directions/xml?key=" + Configuration.Key +
                      "&origin=" + arr[0].Name + "&origin_place_id=" + arr[0].Id +
                      " &destination=" + arr[arr.Length - 1].Name + "&destination_place_id=" + arr[arr.Length - 1].Id +
                      " &waypoints=";

            //add the waypoints
            for (var i = 1; i < arr.Length - 1; i++)
            {
                url += arr[i].Name + "|";
            }
            url = url.TrimEnd('|');

            //add the waypoints id
            url += "&waypoint_place_ids=";
            for (var i = 1; i < arr.Length - 1; i++)
            {
                url += arr[i].Id + "|";
            }
            url = url.TrimEnd('|');

            //download the data
            XElement xml = DownloadDataIntoXml(url);

            //return the sum of the durations
            return (from leg in xml.Elements("route").Elements()
                    where leg.Name == "leg"
                    select int.Parse(leg.Element("duration").Element("value").Value)).Sum();
        }

        /// <summary>
        /// Get the url to show the route on google maps
        /// </summary>
        /// <param name="arr">the address</param>
        /// <returns>an url</returns>
        private static string GetGoogleUrl(GoogleAddress[] arr)
        {
            //create the url for start and end
            var url = "https://www.google.com/maps/dir/?api=1" + "&travelmode=driving" +
                      "&origin=" + arr[0].Name + "&origin_place_id=" + arr[0].Id +
                      "&destination=" + arr[0].Name + "&destination_place_id=" + arr[0].Id +
                      "&waypoints=";

            //add the waypoints
            for (var i = 1; i < arr.Length; i++)
            {
                url += arr[i].Name + "|";
            }
            url = url.TrimEnd('|');

            //add the waypoints id's
            url += "&waypoint_place_ids=";
            for (var i = 1; i < arr.Length; i++)
            {
                url += arr[i].Id + "|";
            }
            url = url.TrimEnd('|');

            return url;
        }

        /// <summary>
        /// download the data from the url address into an xml and check if the google api response was OK
        /// </summary>
        /// <param name="url">the url</param>
        /// <returns>the xml</returns>
        private static XElement DownloadDataIntoXml(string url)
        {
            //check the url
            if (url.ToLower().IndexOf("https:") > -1 || url.ToLower().IndexOf("http:") > -1)
            {
                //download the data into an xml
                var wc = new System.Net.WebClient();
                var response = wc.DownloadData(url);
                var content = System.Text.Encoding.UTF8.GetString(response);
                XElement xml = XElement.Parse(content);

                //check the request state
                if (xml.Element("status").Value != "OK")
                    throw new GoogleAddressException("Google returns the next error: ", xml.Element("status").Value);

                return xml;
            }
            throw new GoogleAddressException("Google URL is not correct", "WRONG_URL");
        }

        #endregion

    }
}
