using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DS;

namespace DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DalImp : IDal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTest"></param>
        public void AddTest(Test newTest)
        {
            newTest.ID = string.Format("{0:00000000}",Configuration.TestID);
            Configuration.TestID++;

            DataSource.Tests.Add(newTest);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTester"></param>
        public void AddTester(Tester newTester)
        {
            if (DataSource.Testers.Any(tester => tester.ID == newTester.ID))
                throw new Exception("the tester already exist in the system");

            DataSource.Testers.Add(newTester);
        }

        public void AddTrainee(Trainee newTrainee)
        {
            if (DataSource.Trainees.Any(t => t.ID == newTrainee.ID))
                throw new Exception("the trainee already exist in the system");

            DataSource.Trainees.Add(newTrainee);
        }

        public IEnumerable<Tester> AllTesters => DataSource.Testers;

        public IEnumerable<Test> AllTests => DataSource.Tests;

        public IEnumerable<Trainee> AllTrainee => DataSource.Trainees;

        public void RemoveTest(Test testToDelete)
        {
            if (DataSource.Tests.All(x => x.ID != testToDelete.ID))
                throw new Exception("Test doesn't exist");

            DataSource.Tests.Remove(testToDelete);
        }

        public void RemoveTester(Tester testerToDelete)
        {
            if (DataSource.Testers.All(x => x.ID != testerToDelete.ID))
                throw new Exception("Tester doesn't exist");

            DataSource.Testers.Remove(testerToDelete);
        }

        public void RemoveTrainee(Trainee traineeToDelete)
        {
            if (DataSource.Trainees.All(x => x.ID != traineeToDelete.ID))
                throw new Exception("Trainee doesn't exist");


            DataSource.Trainees.Remove(traineeToDelete);
        }

        public void UpdateTest(Test updatedTest)
        {
            if (DataSource.Tests.All(x => x.ID != updatedTest.ID))
                throw new Exception("Test doesn't exist");

            var test = DataSource.Tests.Find(t => t.ID == updatedTest.ID);
            DataSource.Tests.Remove(test);
            DataSource.Tests.Add(updatedTest);
        }

        public void UpdateTester(Tester updatedTester)
        {
            if (DataSource.Testers.All(x => x.ID != updatedTester.ID))
                throw new Exception("Tester doesn't exist");

            var tester = DataSource.Testers.Find(t => t.ID == updatedTester.ID);
            DataSource.Testers.Remove(tester);
            DataSource.Testers.Add(updatedTester);
        }

        public void UpdateTrainee(Trainee updatedTrainee)
        {
            if (DataSource.Trainees.All(x => x.ID != updatedTrainee.ID))
                throw new Exception("Trainee doesn't exist");

            var trainee = DataSource.Trainees.Find(t => t.ID == updatedTrainee.ID);
            DataSource.Trainees.Remove(trainee);
            DataSource.Trainees.Add(updatedTrainee);
        }
    }
}
