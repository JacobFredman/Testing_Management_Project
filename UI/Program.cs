﻿using System;
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
                    Console.WriteLine(Tools.GetDistanceGoogleMapsAPI(new Address("ירושלים", "הרצל", "1"), new Address("בית שמש", "הרצל", "621")));
                    Tester t = new Tester(319185997);
                    t.Scedual.AddHoursAllDays(10, 14, 18, 22);
                    Console.WriteLine(t.ToString());
                    Console.WriteLine(t.Scedual.ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine("ERROR: "+ex.Message+"!!\t" + "Source:" +ex.StackTrace.Substring(5,80)+"...");
                }
            }
        }
    }
}