using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BE
{
    /// <summary>
    /// Represent a address
    /// </summary>
    public class Address
    {
        private string id = null;
        public string ID { private set { id = value; }
            get
            {
                if (id != null)
                    return "place_id:" + id;
                else
                    throw new Exception("ERROR: "+ GoogleRequestState);
            }
        }
        private string text;
        public string GoogleRequestState { private set; get; }
        private Thread thread;

        public Address(string address)
        {

            GoogleRequestState = "Unable to reach the internet";
            text = address;
            id = null;
        }
        private void  checkAddress(object ob) {

            string address = ob as string;
            string url = "https://maps.googleapis.com/maps/api/place/findplacefromtext/xml?key=" + 
                Configuration.Key + "&inputtype=textquery&input=" + address + 
                "&fields=formatted_address,place_id"+ "&language="+Configuration.GoogleLanguage;
            if (url.ToLower().IndexOf("https:") > -1 || url.ToLower().IndexOf("http:") > -1)
            {
                try
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(url);
                    string content = System.Text.Encoding.UTF8.GetString(response);
                    XElement xml = XElement.Parse(content);
                    if (xml.Element("status").Value == "OK")
                    {
                        text = xml.Element("candidates").Element("formatted_address").Value;
                        id = xml.Element("candidates").Element("place_id").Value;
                        GoogleRequestState = "OK";
                    }
                    else
                    {
                        id = null;
                        switch (xml.Element("status").Value)
                        {
                            case "ZERO_RESULTS":
                                GoogleRequestState = "Place not found on google maps";
                                break;
                            case "OVER_QUERY_LIMIT":
                                GoogleRequestState = "To many requests to google server";
                                break;
                            case "REQUEST_DENIED":
                                GoogleRequestState = "Invalied google developer key";
                                break;
                            case "INVALID_REQUEST":
                                GoogleRequestState = "Invalied request";
                                break;
                            case "UNKNOWN_ERROR":
                            default:
                                GoogleRequestState = "Unknown error";
                                break;
                        }
                    }
                }catch(Exception ex)
                {
                    id = null;
                    GoogleRequestState = "Unable to reach the internet";
                }
            }
            else
            {
                id = null;
                GoogleRequestState = "Google URL is not correct";
            }

        }
        /// <summary>
        /// check if place exist on google maps
        /// </summary>
        /// <returns></returns>
        public bool PlaceExist()
        {
            return (id != null);
        }
        /// <summary>
        /// Check the address again on google maps. can take time until it changes the values
        /// </summary>
        public bool CheckAddress(bool wait=false)
        {
            thread = new Thread(checkAddress);
            thread.Start(text);
            if(wait)
                thread.Join();
            return PlaceExist();
        }
        /// <summary>
        /// Get the place
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return text;
        }
    }
}
