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
                    BlImp bl = BL.FactoryBl.GetObject;
                    Trainee t = new Trainee(319185997, Gender.Male, "Elisha", "Mayer")
                    {
                        Address = new Address("jerusalem"),
                        BirthDate = new DateTime(1995, 08, 17),
                        Email = "emayer@g.jct.ac.il",
                        PhoneNumber = "0532429933",
                        TesterName = new Tester(319185997),
                        SchoolName = "Gil",
                    };
                    t.LicenceTypeLearning.Add(LicenceType.A);
                    t.LicenceTypeLearning.Add(LicenceType.B);
                    bl.AddTrainee(t);

                    Trainee t2 = new Trainee(319185989, Gender.Male, "Amnon", "Mayer")
                    {
                        Address = new Address("jerusalem","Hetzel","1","23"),
                        BirthDate = new DateTime(1967, 04, 17),
                        Email = "Amnon@g.jct.ac.il",
                        PhoneNumber = "089767006",
                        TesterName = new Tester(319185997),
                        SchoolName = "Gil",
                    };
                    t2.LicenceTypeLearning.Add(LicenceType.C);
                    t2.LicenceTypeLearning.Add(LicenceType._1);
                    bl.AddTrainee(t2);

                    bl.AllTrainee.ToExcel(x=>x.SchoolName=="Gil");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message );
                }

                //Console.ReadKey();
            }
        }
    }
}
