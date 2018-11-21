using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public class BlImp : IBL
    {
        public void AddTester(Tester newTester)
        {
            if (GetAge(newTester.BirthDate) < 40) 
                return;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void RemoveTrainee(Trainee traineeToDelete)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrainee(Trainee updatedTrainee)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Trainee> GetAllTrainee()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Tester> GetAllTesters()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Test> GetAllTests()
        {
            throw new NotImplementedException();
        }
    }
}
