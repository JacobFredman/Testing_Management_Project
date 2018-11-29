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
                
                try {
                    Test t=new Test(319185997,319185989);
                    Console.WriteLine("Enter address: ");
                    t.SetRouteAndAddressToTest(new Address(Console.ReadLine()));
                    Console.WriteLine("Opening chrome...");
                    Routes.ShowUrlInChromeWindow(t.Route.AbsoluteUri);
                   

                }
                catch(GoogleAddressException ex)
                {
                    Console.WriteLine(ex.Message+" Code: "+ex.ErrorCode);
                }
                Console.ReadKey();
            }
        }
    }
}
