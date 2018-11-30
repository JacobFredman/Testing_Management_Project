using System;
using System.Collections.Generic;
using BE;
using BL;

namespace UI
{

    class Program
    {
        


     
        static void Main(string[] args)
        {
            BlImp _blImp = new BlImp();
            
            try
            {
                var birthDate = new DateTime(1985, 12, 29);
                var address = new Address("Jerusalem", "Shachal", "55", "A");
                var licenseTypes = new List<LicenceType>();
                var learnningLicenseTypes = new List<LicenceType>();

                var trainee = new Trainee(037982519, "Jacob", "Fredman", Gender.Male, "jacov141gmail.com", birthDate,
                    "0586300016", address, licenseTypes, learnningLicenseTypes, Gear.Automat, "or Yarok", 18, 23, true);

                _blImp.AddTrainee(trainee);

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
              //  List<Trainee> trainees = _blImp.AllTrainee;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //while (true)
            //{


            //    try
            //    {
            //        Test test = new Test(319185997, 319185997);
            //        Console.WriteLine("enter an address: ");
                    
            //        test.SetRouteAndAddressToTest(new Address(Console.ReadLine()));
            //        Console.WriteLine("lunching chrome....");
            //        Routes.ShowUrlInChromeWindow(test.RouteUrl);
            //    }
            //    catch (GoogleAddressException ex)
            //    {
            //        Console.WriteLine(ex.Message + " Code: " + ex.ErrorCode);
            //    }

            //    //Console.ReadKey();
            //}
        }
    }
}
