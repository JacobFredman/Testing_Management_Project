using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;
using Exception = System.Exception;

namespace BL
{
    public class BlImp : IBL
    {
        private readonly DalImp _dalImp = FactoryDal.GetObject;

        #region Access Tester
        /// <summary>
        /// Add a Tester
        /// </summary>
        /// <param name="newTester">The Tester to add</param>
        public void AddTester(Tester newTester)
        {
            if (GetAge(newTester.BirthDate) < Configuration.MinTesterAge) 
                throw new Exception("the Tester is too young");

            _dalImp.AddTester(newTester);
        }

        /// <summary>
        /// Remove a Tester
        /// </summary>
        /// <param name="testerToDelete">The Tester to remove</param>
        public void RemoveTester(Tester testerToDelete)
        {
            _dalImp.RemoveTester(testerToDelete);
        }

        /// <summary>
        /// Update Tester
        /// </summary>
        /// <param name="updatedTester">The Tester to update</param>
        public void UpdateTester(Tester updatedTester)
        {
            if (GetAge(updatedTester.BirthDate) < Configuration.MinTesterAge)
                throw new Exception("the Tester is too young");

            _dalImp.UpdateTester(updatedTester);
        }
        #endregion

        #region Access Test
        /// <summary>
        /// Add a Test
        /// </summary>
        /// <param name="newTest">The Test to add</param>
        public void AddTest(Test newTest)
        {
            var traineeExist = AllTrainee.Any(trainee => trainee.ID == newTest.TraineeId);
            var twoTestesTooClose = AllTests.Any(test => (test.TraineeId == newTest.TraineeId) && ((newTest.Date - test.Date).TotalDays < Configuration.MinTimeBetweenTests));
            var lessThenMinLessons = AllTrainee.Any(trainee => (trainee.ID == newTest.TraineeId) && trainee.NumberOfLessons < Configuration.MinLessons);
            var traineeHasLicense = AllTrainee.Any(trainee =>
                (trainee.ID == newTest.TesterId) && (trainee.LicenceType.Any(l => l == newTest.LicenceType)));
            var testerHasLicense = AllTesters.Any(tester => 
                (tester.ID == newTest.TesterId) && (tester.LicenceType.Any(l => l == newTest.LicenceType)));

            var traineeHasTestInSameTime = AllTests.Any(test => (test.TraineeId == newTest.TraineeId) && (newTest.Date == test.Date));
            var testerHasTestInSameTime = AllTests.Any(test => (test.TesterId == newTest.TesterId) && (newTest.Date == test.Date));

            if(!traineeExist) throw new Exception("this trainee doesn'trainee exist");
            if (twoTestesTooClose) throw  new Exception("the trainee has a test less then a week ago");
            if(lessThenMinLessons) throw new  Exception("the trainee learned less then " + Configuration.MinLessons + " lessons which is the minimum");
            if(traineeHasLicense) throw  new Exception("the trainee has already a license with same type");
            if (!testerHasLicense) throw new Exception("tester is not qualified for this license type");
            if(traineeHasTestInSameTime) throw  new Exception("the trainee has already another test in the same time");
            if(testerHasTestInSameTime) throw  new Exception("the tester has already another test in the same time");

            _dalImp.AddTest(newTest);
        }

        /// <summary>
        /// Remove a Test
        /// </summary>
        /// <param name="testToDelete">The Test to remove</param>
        public void RemoveTest(Test testToDelete)
        {
            _dalImp.RemoveTest(testToDelete);
        }

        /// <summary>
        /// Update Test
        /// </summary>
        /// <param name="updatedTest">The Test to update</param>
        public void UpdateTest(Test updatedTest)
        {
            if (AllTests.All(test => test.Code != updatedTest.Code))
                throw new Exception("Test doesn'trainee exist");
            if (updatedTest.Criterions.Count <= Configuration.MinimumCritirions)
                throw new Exception("not enough criterion");
            if(updatedTest.ActualDateTime==DateTime.MinValue)
                throw new Exception("test date not updated");
            UpdatePassTest(updatedTest);
            _dalImp.UpdateTest(updatedTest);

        }
        #endregion

        #region Access Trainee

        /// <summary>
        /// Add Trainee
        /// </summary>
        /// <param name="newTrainee">The Trainee to add</param>
        public void AddTrainee(Trainee newTrainee)
        {
            if (GetAge(newTrainee.BirthDate) < Configuration.MinTraineeAge)
                throw new Exception("the trainee is too young");

            _dalImp.AddTrainee(newTrainee);
        }

        /// <summary>
        /// Remove Trainee
        /// </summary>
        /// <param name="traineeToDelete">The Trainee to add</param>
        public void RemoveTrainee(Trainee traineeToDelete)
        {
            _dalImp.RemoveTrainee(traineeToDelete);
        }

        /// <summary>
        /// Update Trainee
        /// </summary>
        /// <param name="updatedTrainee">The Trainee to update</param>
        public void UpdateTrainee(Trainee updatedTrainee)
        {
            if (GetAge(updatedTrainee.BirthDate) < Configuration.MinTraineeAge)
                throw new Exception("the trainee is too young");
            _dalImp.UpdateTrainee(updatedTrainee);
        }
        #endregion

        #region Get list's

        /// <summary>
        /// Get All Trainee's
        /// </summary>
        public IEnumerable<Trainee> AllTrainee => _dalImp.AllTrainee;

        /// <summary>
        /// Get all Tester's
        /// </summary>
        public IEnumerable<Tester> AllTesters => _dalImp.AllTesters;

        /// <summary>
        /// Get all Test's
        /// </summary>
        public IEnumerable<Test> AllTests => _dalImp.AllTests;

        /// <summary>
        /// Get all the Testers that are available on the date
        /// </summary>
        /// <param name="date">Checks if the teacher is available on the given day and hour</param>
        /// <returns>List of Testers</returns>
        public IEnumerable<Tester> GetAvailableTesters(DateTime date)
        {
            return AllTesters.Where(tester =>
                (tester.Scedule.IsAvailable(date.DayOfWeek, date.Hour)) &&
                !(AllTests.Any(test =>
                        (test.TesterId == tester.ID && test.Date.DayOfWeek == date.DayOfWeek && test.Date.Hour == date.Hour))
                    )
            );
        }

        /// <summary>
        /// Get all Tests sorted by Date
        /// </summary>
        /// <returns>List of Tests</returns>
        public IEnumerable<Test> GetAllTestsSortedByDate()
        {
            return AllTests.OrderBy(x => x.Date);
        }

        /// <summary>
        /// Get all tests where the function returns true
        /// </summary>
        /// <param name="func">A func that gets a Test and returns bool</param>
        /// <returns>List of Tests</returns>
        public IEnumerable<Test> GetAllTestsWhere(Func<Test, bool> func)
        {
            return AllTests.Where(func);
        }

        /// <summary>
        /// Get all the testers that are in a specified distance from an address.
        /// This function uses requests from internet. in can take a long time ,so it is recommended to use in a separate thread.
        /// </summary>
        /// <param name="r">the distance</param>
        /// <param name="a">the address</param>
        /// <returns></returns>
        public IEnumerable<Tester> GetAllTestersInRadios(int r, Address a)
        {
            return AllTesters.Where(tester => Tools.GetDistanceGoogleMapsAPI(tester.Address, a) <= r);
        }
        #endregion

        #region Grouping

        /// <summary>
        /// Get all Tester's grouped by License
        /// </summary>
        /// <returns>All Tester's grouped by license</returns>
        public IEnumerable<IGrouping<List<LicenceType>, Tester>> GetAllTestersByLicense()
        {
            return AllTesters.GroupBy(x => x.LicenceTypeTeaching);
        }

        /// <summary>
        /// Get all Trainee's grouped by Their Tester's 
        /// </summary>
        /// <returns>All Trainee's grouped by Their Tester's </returns>
        public IEnumerable<IGrouping<Tester, Trainee>> GetAllTraineesByTester()
        {
            return from trainee in AllTrainee
                group trainee by trainee.TesterName;
        }

        /// <summary>
        /// Get all Trainee's grouped by Their school's 
        /// </summary>
        /// <returns>All Trainee's grouped by Their school's </returns>
        public IEnumerable<IGrouping<string, Trainee>> GetAllTraineesBySchool()
        {
            return from trainee in AllTrainee
                group trainee by trainee.SchoolName;
        }

        /// <summary>
        /// Get all Trainee's grouped by Their number of test's
        /// </summary>
        /// <returns>All Trainee's grouped by Their number of test's</returns>
        public IEnumerable<IGrouping<int, Trainee>> GetAllTraineeByNumberOfTests()
        {
            return (from trainee in AllTrainee
                group trainee by GetNumberOfTests(trainee));
        }
        #endregion

        #region Help Function's

        /// <summary>
        /// Update the test if the Trainee passed according to the criterion
        /// </summary>
        /// <param name="test">The Test</param>
        private static void UpdatePassTest(Test test)
        {
            var percent = test.Criterions.Count(x => x.Pass) / (double)test.Criterions.Count;
            test.passed = (percent >= Configuration.PersentOfCritirionsToPassTest);
           
        }

        /// <summary>
        /// Get an age from a birth date
        /// </summary>
        /// <param name="birthDate">The birth date</param>
        /// <returns>The age in years</returns>
        private static int GetAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        #endregion
 
        /// <summary>
        /// Get the number of tests that the Trainee did </summary>
        /// <param name="trainee">The Trainee</param>
        /// <returns>The number of Tests</returns>
        public int GetNumberOfTests(Trainee trainee)
        {
           return AllTests.Count(x => x.TraineeId == trainee.ID && x.ActualDateTime > DateTime.Now.Date);
        }

        /// <summary>
        /// Check if Trainee passed the test
        /// </summary>
        /// <param name="trainee">The Trainee</param>
        /// <param name="license">The license</param>
        /// <returns>True if he passed</returns>
        public bool TraineePassedTest(Trainee trainee,LicenceType license)
        {
            return AllTests.Any(test => test.TesterId == trainee.ID && test.LicenceType == license && test.passed);
        }







 
    }

}
