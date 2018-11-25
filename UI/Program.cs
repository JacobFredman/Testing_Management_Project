using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    //Console.WriteLine(Tools.GetDistanceGoogleMapsAPI(new Address("ירושלים", "הרצל", "1"), new Address("בית שמש", "הרצל", "621")));
                    Tester t = new Tester(319185997);
                    t.Email = "emayer@gamil.com";
                    t.Scedule.AddHoursAllDays(10, 14, 18, 22);
                    Console.WriteLine(t.Scedule.IsAvailable(DayOfWeek.Sunday,8).ToString());
                    Console.WriteLine(t.Scedule.ToString());
                    BL.BlImp bl = new BlImp();
                    bl.AddTester(t);
                    Tester t2 = new Tester(319185989, "elisja", "mayer", Gender.Male);
                    t2.Scedule.AddHoursAllDays(11, 11);
                    bl.AddTester(t2);
                    foreach(Tester  tester in bl.GetAvailableTesters(new DateTime(2018, 11, 22, 19, 00, 00))){
                        Console.WriteLine(tester);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("ERROR: "+ex.Message+"!!\t" + "Source:" +ex.StackTrace.Substring(5,80)+"...");
                }
            }
        }
    }
}
