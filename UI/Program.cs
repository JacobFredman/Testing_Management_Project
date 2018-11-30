using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using BL;

namespace UI
{

    class Program
    {
        


     
        static void Main(string[] args)
        {
            BlImp _blImp = new BlImp();
            Trainee trainee, trainee2;
            Tester tester1;
            Test test;
            try
            {

                // add trainne1
                var birthDate = new DateTime(1985, 12, 29);
                var address = new Address("Jerusalem", "Shachal", "55", "A");
                var licenseTypes = new List<LicenceType>();
                var learnningLicenseTypes = new List<LicenceType>();

                 trainee = new Trainee(037982519, "Jacob", "Fredman", Gender.Male, "jacov141@gmail.com", birthDate,
                    "0586300016", address, licenseTypes, learnningLicenseTypes, Gear.Automat, "or Yarok", 18, 23, true);

                _blImp.AddTrainee(trainee);


                // add trainne2
                var birthDate2 = new DateTime(1999, 12, 29);
                var address2 = new Address("Jerusalem", "King george", "55", "A");
                var licenseTypes2 = new List<LicenceType>();
                var learnningLicenseTypes2 = new List<LicenceType>();

                 trainee2 = new Trainee(300391737, "Elisha", "Mayer", Gender.Male, "elisha@gmail.com", birthDate2,
                    "0586340000", address2, licenseTypes2, learnningLicenseTypes2, Gear.Manual, "or Yarok", 12, 20, true);

              
                _blImp.AddTrainee(trainee2);


                // add tester1
                var birthDateT = new DateTime(1975, 12, 29);
                var addressT = new Address("Jerusalem", "King george", "55", "A");
                var licenseTypesT = new List<LicenceType>();
                var TeachingLicenseTypesT = new List<LicenceType>();
                tester1 = new Tester(223555616, "moshe", "levi", Gender.Male, "moshe@gmail.com", birthDateT,
                    "0586341111", addressT, licenseTypesT,18,20, TeachingLicenseTypesT,8);
                _blImp.AddTester(tester1);



                //test = new Test(223555616, 300391737);
                //_blImp.AddTest(test);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }




            try
            {
              //  _blImp.RemoveTrainee(trainee);
                var trainees = _blImp.AllTrainee.ToList();
                foreach (var traineee in trainees)
                {
                    Console.WriteLine(trainee.ToString());
                }

                var testers = _blImp.AllTesters.ToList();
                foreach (var tester in testers)
                {
                    Console.WriteLine(tester.ToString());
                }

                var testes = _blImp.AllTests.ToList();
                foreach (var test1 in testers)
                {
                    Console.WriteLine(test1.ToString());
                }
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
