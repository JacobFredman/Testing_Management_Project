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
        private readonly DalImp _dalImp = new DalImp();
        public void AddTester(Tester newTester)
        {
            if (GetAge(newTester.BirthDate) < Configuration.MinTesterAge) 
                throw new Exception("the Tester is too young");

            _dalImp.AddTester(newTester);
        }

        public void RemoveTester(Tester testerToDelete)
        {
            throw new NotImplementedException();
        }

        public void UpdateTester(Tester updatedTester)
        {
            throw new NotImplementedException();
        }

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

            if(!traineeExist) throw new Exception("this trainee doesn't exist");
            if (twoTestesTooClose) throw  new Exception("the trainee has a test less then a week ago");
            if(lessThenMinLessons) throw new  Exception("the trainee learned less then " + Configuration.MinLessons + " lessons which is the minimum");
            if(traineeHasLicense) throw  new Exception("the trainee has already a license with same type");
            if (!testerHasLicense) throw new Exception("tester is not qualified for this license type");
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
            throw new NotImplementedException();
        }

        public void UpdateTrainee(Trainee updatedTrainee)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfTests(Trainee trainee)
        {
           return AllTests.Count(x => x.TraineeId == trainee.ID && x.Date > DateTime.Now.Date);
        }
       
        #region Get list's
        public List<Tester> GetAvailableTesters(DateTime date)
        {
            return AllTesters.Where(tester =>
                (tester.Scedule[date.DayOfWeek].IsWorking(date.Hour)) &&
                !(AllTests.Any(test =>
                    (test.TesterId == tester.ID && test.Date.DayOfWeek == date.DayOfWeek && test.Date.Hour == date.Hour))
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

        public List<Tester> GetAllTestersInRadios(Tester t, int r, Address a)
        {
            return AllTesters.Where(tester => Tools.GetDistanceGoogleMapsAPI(tester.Address, a) <= r).ToList();
        }
        #endregion

        public List<Trainee> AllTrainee => _dalImp.AllTrainee;
        public List<Tester> AllTesters => _dalImp.AllTesters;
        public List<Test> AllTests => _dalImp.AllTests;

        #region Grouping
        IEnumerable<IGrouping<List<LicenceType>, Tester>> AllTestersByLicense => AllTesters.GroupBy(x => x.LicenceTypeTeaching);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGrouping<Tester, Trainee>> AllTraineesByTester => from trainee in AllTrainee group trainee by trainee.TesterName;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGrouping<string, Trainee>> AllTraineesBySchool => from trainee in AllTrainee group trainee by trainee.SchoolName;

        IEnumerable<IGrouping<int, Trainee>> AllTraineeByNumberOfTests => (from trainee in AllTrainee group trainee by GetNumberOfTests(trainee));
        #endregion

        private void UpdatePassTest(Test t)
        {
            double pers = (double)t.Criterions.Count(x => x.Pass) / t.Criterions.Count;
            t.passed = (pers >= Configuration.PersentgeOfCritirionsToPassTest) ;
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
