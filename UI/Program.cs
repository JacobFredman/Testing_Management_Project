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
                    Tester t1 = new Tester(319185997,"Elisha","Mayer",Gender.Male);
                    Tester t2 = new Tester(319185989,"Yakov","Fridman",Gender.Male);
                    t1.Address = new Address("מכון לב");
                    t2.Address = new Address("בית שמש הרצל 631");
                    Address t = new Address("טלסטון");
                    ;
                    ;
                    ;
                    //Thread.Sleep(7000);
                    Console.WriteLine(t1.Address.CheckAddress(true).ToString()+" "+ 
                        t2.Address.CheckAddress(true).ToString()+" "+ 
                        t.CheckAddress(true).ToString());

                    Console.WriteLine(t1.Address.GoogleRequestState);
                    Console.WriteLine(t2.Address.GoogleRequestState);
                    Console.WriteLine(t.GoogleRequestState);


                    Console.WriteLine(t1.Address);
                    Console.WriteLine(t2.Address);
                    Console.WriteLine(t);

                    Route r = new Route(t1.Address.ToString(),t.ToString(), t2.Address.ToString());
                    Console.WriteLine(r.GetGoogleURL());
                    

                    //t1.Schedule.AddHoursAllDays(10, 14, 18, 22);
                    //t1.Schedule.AddHourToDay(Days.Sunday, 4, 6);
                    //t1.Schedule[Days.Thursday].RemoveHours(11, 12);


                    //Console.WriteLine(t1.ToString());
                    //Console.WriteLine(t1.Schedule.ToString());
                    //Address[] ad =new[] { new Address("Hemed Interchange"), new Address("shoresh Interchange") };
                    //Console.WriteLine(Tools.GetMapWayPointURL(t1.Address,t2.Address, new[] { new Address("Hemed Interchange"), new Address("shoresh Interchange") }));

                    //Console.WriteLine(Tools.GetDistanceGoogleMapsAPIXML(t1.Address, t2.Address) / 1000 + " km");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(Tools.GetExpectionMEssage(ex,true,120));
                }
                Console.ReadKey();
            }
        }
    }
}
