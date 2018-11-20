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
        private string text;
        private Thread thread;

        public Address(string address)
        {
            text = address;
            thread = new Thread(checkAddress);
            thread.Start(address);
        }
        private void  checkAddress(object ob) {

            string address = ob as string;
            string url = "https://maps.googleapis.com/maps/api/place/findplacefromtext/xml?key=" + Configuration.Key + "&inputtype=textquery&input=" + address + "&fields=formatted_address,place_id";
            if (url.ToLower().IndexOf("https:") > -1 || url.ToLower().IndexOf("http:") > -1)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(url);
                string content = System.Text.Encoding.ASCII.GetString(response);
                XElement xml = XElement.Parse(content);
                if (xml.Element("status").Value == "OK")
                {
                    text = xml.Element("candidates").Element("formatted_address").Value;
                    id = xml.Element("candidates").Element("place_id").Value;
                }
                else
                {
                    id = null;
                }
            }
            else
                throw new Exception("Google URL is not correct");

        }

        public string getID()
        {
            if (id != null)
                return "place_id:" + id;
            else
                throw new Exception("place not found");
        }

        public bool PlaceExist()
        {
            return (id != null);
        }

        

        public override string ToString()
        {
            return text;
        }
    }
}
