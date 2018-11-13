using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try {
                    Tester t = new Tester(319185998);
                    t.Scedual.AddHoursAllDays(10, 14, 18, 22,1);
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
