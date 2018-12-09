using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using BE.MainObjects;
using BE.Routes;
using BL;

namespace UI
{

    class Program
    {
        
    
        static void Main(string[] args)
        {
         
            BlImp _blImp = FactoryBl.GetObject;
          //  Trainee trainee, trainee2;
          //  Tester tester1;
          // Test test;
           Email email = new  Email();
           // Pdf pdf = new Pdf();

            try
            {

                AddTrainee1(_blImp);
                AddTrainee2(_blImp);

                AddTester1(_blImp);

                AddTest(_blImp);

              var testA =  _blImp.AllTests.First();
                var traineeA = _blImp.AllTrainee.First();

                //   email.SentEmailToTraineeBeforeTest(testA,traineeA);
                //   email.SentEmailToTraineeAfterTest(testA,traineeA);
                // Pdf.CreateDocument(_blImp.AllTests.First,);
                Pdf.CreateLicensePdf(_blImp.AllTests.First(), _blImp.AllTrainee.First());

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
                foreach (var trainee in trainees)
                {
                    Console.WriteLine(trainee.ToString());
                }

                var testers = _blImp.AllTesters.ToList();
                foreach (var tester in testers)
                {
                    Console.WriteLine(tester.ToString());
                }

                var testes = _blImp.AllTests.ToList();
                foreach (var test1 in testes)
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

        private static void AddTest(BlImp _blImp)
        {
            Test test;
            DateTime testTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1);
            Address testAddress = new Address("jerusalem", "havaad aluemi", "3", "A");
            List<Criterion> cariteria = new List<Criterion>();
            cariteria.Add(new Criterion("looking in mirrors", true));


            test = new Test(223555616, 037982519, testTime, testAddress, cariteria, true, "0", LicenseType.A);

            _blImp.AddTest(test);
        }

        private static void AddTester1(BlImp _blImp)
        {
            Tester tester1;
            // add tester1
            var birthDateT = new DateTime(1975, 12, 29);
            var addressT = new Address("Jerusalem", "King george", "55", "A");
            var licenseTypesT = new List<LicenseType>();
            var TeachingLicenseTypesT = new List<LicenseType>();
            TeachingLicenseTypesT.Add(LicenseType.A);
            TeachingLicenseTypesT.Add(LicenseType.A);
            tester1 = new Tester(223555616, "moshe", "levi", Gender.Male, "moshe@gmail.com", birthDateT,
                "0586341111", addressT, licenseTypesT, 18, 20, TeachingLicenseTypesT, 8);
            _blImp.AddTester(tester1);
        }

        private static void  AddTrainee2(BlImp _blImp)
        {
            Trainee trainee2;
            // add trainne2
            var birthDate2 = new DateTime(1999, 12, 29);
            var address2 = new Address("Jerusalem", "King george", "55", "A");
            var licenseTypes2 = new List<LicenseType>();
            var learningLicenseTypes2 = new List<LessonsAndType>();
            learningLicenseTypes2.Add(new LessonsAndType { License = LicenseType.A, NumberOfLessons = 30 });

            trainee2 = new Trainee(300391737, "Elisha", "Mayer", Gender.Male, "elisha@gmail.com", birthDate2,
               "0586340000", address2, licenseTypes2, learningLicenseTypes2, Gear.Manual, "or Yarok", "12", 20, true);


            _blImp.AddTrainee(trainee2);
        }

        private static void  AddTrainee1(BlImp _blImp)
        {
            Trainee trainee;
            // add trainne1
            var birthDate = new DateTime(1985, 12, 29);
            var address = new Address("Jerusalem", "Shachal", "55", "A");
            var licenseTypes = new List<LicenseType>();
            var learnningLicenseTypes = new List<LessonsAndType>();
            learnningLicenseTypes.Add(new LessonsAndType { License = LicenseType.A, NumberOfLessons = 30 });

            trainee = new Trainee(037982519, "Jacob", "Fredman", Gender.Male, "jacov141@gmail.com", birthDate,
               "0586300016", address, licenseTypes, learnningLicenseTypes, Gear.Automatic, "or Yarok", "18", 23, true);

            _blImp.AddTrainee(trainee);
        }
    }
}
