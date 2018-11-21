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
        DalImp _dalImp = new DalImp();
        public void AddTester(Tester newTester)
        {
            if (GetAge(newTester.BirthDate) < Configuration.MinTesterAge) 
                throw new Exception("the Tester is too young");

            _dalImp.AddTester(newTester);
        }

     
        private static int GetAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
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
            var towTestesTooClose = AllTests.Any(test => (test.TraineeId == newTest.TraineeId) && ((newTest.Date - test.Date).TotalDays < Configuration.MinTimeBetweenTests));
            var lessThenMinLessons = AllTrainee.Any(trainee => (trainee.ID == newTest.TraineeId) && trainee.NumberOfLessons < Configuration.MinLessons);
            var hasLicense = AllTrainee.Any(trainee =>
                (trainee.ID == newTest.TesterId) && (trainee.LicenceType.Any(l => l == newTest.LicenceType)));

            var traineeHasTestInSameTime = AllTests.Any(test => (test.TraineeId == newTest.TraineeId) && (newTest.Date == test.Date));
            var testerHasTestInSameTime = AllTests.Any(test => (test.TesterId == newTest.TesterId) && (newTest.Date == test.Date));

            if(!traineeExist) throw new Exception("this trainee doesn't exist");
            if (towTestesTooClose) throw  new Exception("the trainee has a test less then a week ago");
            if(lessThenMinLessons) throw new  Exception("the trainee learned less then " + Configuration.MinLessons + " lessons which is the minimum");
            if(hasLicense) throw  new Exception("the trainee has already a license with same type");
            if(traineeHasTestInSameTime) throw  new Exception("the trainee has already another test in the same time");
            if(testerHasTestInSameTime) throw  new Exception("the tester has already another test in the same time");

            _dalImp.AddTest(newTest);
        }

       

        public void RemoveTest(Test testToDelete)
        {
            throw new NotImplementedException();
        }

        public void UpdateTest(Test updatedTest)
        {
            throw new NotImplementedException();
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

        public List<Trainee> AllTrainee => _dalImp.AllTrainee;
        public List<Tester> AllTesters => _dalImp.AllTesters;
        public List<Test> AllTests => _dalImp.AllTests;

    }
}
