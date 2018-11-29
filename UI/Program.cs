using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BE;
using BL;

namespace UI
{

    class Program
    {


        static void Main(string[] args)
        {
            while (true)
            {

                try
                {
                    Test test = new Test(319185997, 319185997);
                    Console.WriteLine("enter an address: ");
                    
                    test.SetRouteAndAddressToTest(new Address(Console.ReadLine()));
                    Console.WriteLine("lunching chrome....");
                    Routes.ShowUrlInChromeWindow(test.RouteUrl);
                }
                catch (GoogleAddressException ex)
                {
                    Console.WriteLine(ex.Message + " Code: " + ex.ErrorCode);
                }

                //Console.ReadKey();
            }
        }
    }
}
