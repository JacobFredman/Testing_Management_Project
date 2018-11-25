using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Exception = System.Exception;

namespace BL
{
    public class BlImp : IBL
    {
        private readonly DalImp _dalImp = new DalImp();
        public void AddTester(Tester newTester)
        {
            if (GetAge(newTester.BirthDate) < Configuration.MinTesterAge) 
                throw new Exception("the Tester is too young");

            _dalImp.AddTester(newTester);
        }

     
 

        public void RemoveTester(Tester testerToDelete)
        {
            _dalImp.RemoveTester(testerToDelete);
        }

        public void UpdateTester(Tester updatedTester)
        {
            if (GetAge(updatedTester.BirthDate) < Configuration.MinTesterAge)
                throw new Exception("the Tester is too young");

            _dalImp.UpdateTester(updatedTester);
        }

        public void AddTest(Test newTest)
        {
            var traineeExist = AllTrainee.Any(trainee => trainee.ID == newTest.TraineeId);
            var twoTestesTooClose = AllTests.Any(test => (test.TraineeId == newTest.TraineeId) && ((newTest.Date - test.Date).TotalDays < Configuration.MinTimeBetweenTests));
            var lessThenMinLessons = AllTrainee.Any(trainee => (trainee.ID == newTest.TraineeId) && trainee.NumberOfLessons < Configuration.MinLessons);
            var TraineehasLicense = AllTrainee.Any(trainee =>
                (trainee.ID == newTest.TesterId) && (trainee.LicenceType.Any(l => l == newTest.LicenceType)));
            var TesterHasLicecnce = AllTesters.Any(tester => 
                (tester.ID == newTest.TesterId) && (tester.LicenceType.Any(l => l == newTest.LicenceType)));

            var traineeHasTestInSameTime = AllTests.Any(test => (test.TraineeId == newTest.TraineeId) && (newTest.Date == test.Date));
            var testerHasTestInSameTime = AllTests.Any(test => (test.TesterId == newTest.TesterId) && (newTest.Date == test.Date));

            if(!traineeExist) throw new Exception("this trainee doesn't exist");
            if (twoTestesTooClose) throw  new Exception("the trainee has a test less then a week ago");
            if(lessThenMinLessons) throw new  Exception("the trainee learned less then " + Configuration.MinLessons + " lessons which is the minimum");
            if(TraineehasLicense) throw  new Exception("the trainee has already a license with same type");
            if (!TesterHasLicecnce) throw new Exception("tester is not qualified for this license type");
            if(traineeHasTestInSameTime) throw  new Exception("the trainee has already another test in the same time");
            if(testerHasTestInSameTime) throw  new Exception("the tester has already another test in the same time");

            _dalImp.AddTest(newTest);
        }

       

        public void RemoveTest(Test testToDelete)
        {
            _dalImp.RemoveTest(testToDelete);
        }

        public void UpdateTest(Test updatedTest)
        {
            if (!AllTests.Any(test => test.Code == updatedTest.Code))
                throw new Exception("Test doesn't exisit");
            if (updatedTest.Criterions.Count <= Configuration.MinimumCritirions)
                throw new Exception("not enough critirions");
            if(updatedTest.ActualDateTime==DateTime.MinValue)
                throw new Exception("test date not updated");
            UpdatePassTest(updatedTest);
            _dalImp.UpdateTest(updatedTest);

        }

        public void AddTrainee(Trainee newTrainee)
        {
            if (GetAge(newTrainee.BirthDate) < Configuration.MinTraineeAge)
                throw new Exception("the trainee is too young");

            _dalImp.AddTrainee(newTrainee);
        }

        public void RemoveTrainee(Trainee traineeToDelete)
        {
            _dalImp.RemoveTrainee(traineeToDelete);
        }

        public void UpdateTrainee(Trainee updatedTrainee)
        {
            if (GetAge(updatedTrainee.BirthDate) < Configuration.MinTraineeAge)
                throw new Exception("the trainee is too young");
            _dalImp.UpdateTrainee(updatedTrainee);
        }

 
       
        public int GetNumberOfTests(Trainee trainee)
        {
           return AllTests.Count(x => x.TraineeId == trainee.ID && x.Date > DateTime.Now.Date);
        }
        public bool TraineePassedTest(Trainee t,LicenceType l)
        {
            return AllTests.Any(test => test.TesterId == t.ID && test.LicenceType == l && test.Pass);
        }

        #region Get list's
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Date">Checks only if the teacher is avalble on the given day and hour</param>
        /// <returns></returns>
        public List<Tester> GetAvailableTesters(DateTime Date)
        {
            return AllTesters.Where(tester =>
                (tester.Scedule.IsAvailable(Date.DayOfWeek,Date.Hour)) &&
                !(AllTests.Any(test =>
                    (test.TesterId == tester.ID && test.Date.DayOfWeek == Date.DayOfWeek && test.Date.Hour == Date.Hour))
                    )
            ).ToList();
        }
        
        public List<Test> GetAllTestsSortedByDate()
        {
            return AllTests.OrderBy(x => x.Date).ToList();
        }

        public List<Test> GetAllTestsWhere(Func<Test, bool> func)
        {
            return AllTests.Where(func).ToList();
        }
        /// <summary>
        /// Get all the testers that are in a specified distance from an address.
        /// This function uses requests from internet. in can take a long time ,so it is recommendet to use in a seperate thread.
        /// </summary>
        /// <param name="r">the distance</param>
        /// <param name="a">the address</param>
        /// <returns></returns>
        public List<Tester> GetAllTestersInRadios(int r, Address a)
        {
            return AllTesters.Where(tester => Tools.GetDistanceGoogleMapsAPI(tester.Address, a) <= r).ToList();
        }
        #endregion

        public List<Trainee> AllTrainee => _dalImp.AllTrainee;
        public List<Tester> AllTesters => _dalImp.AllTesters;
        public List<Test> AllTests => _dalImp.AllTests;



        #region Grouping
        public IEnumerable<IGrouping<List<LicenceType>,Tester>> GetAllTestersByLicense()
        {
            return AllTesters.GroupBy(x => x.LicenceTypeTeaching);
        }
        public IEnumerable<IGrouping<Tester,Trainee>> GetAllTraineesByTester()
        {
            return from trainee in AllTrainee
                   group trainee by trainee.TesterName;
        }
        public IEnumerable<IGrouping<string, Trainee>> GetAllTraineesBySchool()
        {
            return from trainee in AllTrainee
                   group trainee by trainee.SchoolName;
        }

        public IEnumerable<IGrouping<int,Trainee>> GetAllTraineeByNumberOfTests()
        {
            return (from trainee in AllTrainee
                    group trainee by GetNumberOfTests(trainee));
        }
        #endregion

        private static void UpdatePassTest(Test t)
        {
            var percent = (double)t.Criterions.Count(x => x.Pass) / (double)t.Criterions.Count;
            t.Pass = (percent >= Configuration.PersentOfCritirionsToPassTest) ? true : false;
        }

        private static int GetAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

    }
}
