using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public class BlImp : IBL
    {
        DalImp _dalImp = new DalImp();
        public void AddTester(Tester newTester)
        {
            if (GetAge(newTester.BirthDate) < 40) 
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
            var tested = AllTests.Any(test => (test.Id == newTest.Id) && ((newTest.Date - test.Date).TotalDays < 7));
            var lessThen20Lessons = AllTests.Any(test => (test.Id == newTest.Id) && ((newTest.Date - test.Date).TotalDays < 7));


            if (tested)
                throw  new Exception("the trainee has a test less then a week ago");
     
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
            if (GetAge(newTrainee.BirthDate) < 18)
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
