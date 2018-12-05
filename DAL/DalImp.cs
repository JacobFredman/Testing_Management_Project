using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DS;

namespace DAL
{
    /// <inheritdoc />
    /// <summary>
    /// DAL implantation
    /// </summary>
    public class DalImp : IDal
    {
        #region Test

        /// <summary>
        /// Add a new test
        /// </summary>
        /// <param name="newTest"></param>
        public void AddTest(Test newTest)
        {
            newTest.ID = $"{Configuration.TestID:00000000}";
            Configuration.TestID++;

            DataSource.Tests.Add(newTest);
        }

        /// <summary>
        /// remove a test
        /// </summary>
        /// <param name="testToDelete"></param>
        public void RemoveTest(Test testToDelete)
        {
            if (DataSource.Tests.All(x => x.ID != testToDelete.ID))
                throw new Exception("Test doesn't exist");

            DataSource.Tests.Remove(testToDelete);
        }

        /// <summary>
        /// update an existing test
        /// </summary>
        /// <param name="updatedTest"></param>
        public void UpdateTest(Test updatedTest)
        {
            if (DataSource.Tests.All(x => x.ID != updatedTest.ID))
                throw new Exception("Test doesn't exist");

            var test = DataSource.Tests.Find(t => t.ID == updatedTest.ID);
            DataSource.Tests.Remove(test);
            DataSource.Tests.Add(updatedTest);
        }

        #endregion

        #region Tester

        /// <summary>
        /// Add tester
        /// </summary>
        /// <param name="newTester"></param>
        public void AddTester(Tester newTester)
        {
            if (DataSource.Testers.Any(tester => tester.ID == newTester.ID))
                throw new Exception("the tester already exist in the system");

            DataSource.Testers.Add(newTester);
        }

        /// <summary>
        /// Remove a tester
        /// </summary>
        /// <param name="testerToDelete"></param>
        public void RemoveTester(Tester testerToDelete)
        {
            if (DataSource.Testers.All(x => x.ID != testerToDelete.ID))
                throw new Exception("Tester doesn't exist");

            DataSource.Testers.Remove(testerToDelete);
        }

        /// <summary>
        /// update existing Tester
        /// </summary>
        /// <param name="updatedTester"></param>
        public void UpdateTester(Tester updatedTester)
        {
            if (DataSource.Testers.All(x => x.ID != updatedTester.ID))
                throw new Exception("Tester doesn't exist");

            var tester = DataSource.Testers.Find(t => t.ID == updatedTester.ID);
            DataSource.Testers.Remove(tester);
            DataSource.Testers.Add(updatedTester);
        }

        #endregion

        #region Trainee

        /// <summary>
        /// Add trainee
        /// </summary>
        /// <param name="newTrainee"></param>
        public void AddTrainee(Trainee newTrainee)
        {
            if (DataSource.Trainees.Any(t => t.ID == newTrainee.ID))
                throw new Exception("the trainee already exist in the system");

            DataSource.Trainees.Add(newTrainee);
        }

        /// <summary>
        /// remove trainee
        /// </summary>
        /// <param name="traineeToDelete"></param>
        public void RemoveTrainee(Trainee traineeToDelete)
        {
            if (DataSource.Trainees.All(x => x.ID != traineeToDelete.ID))
                throw new Exception("Trainee doesn't exist");


            DataSource.Trainees.Remove(traineeToDelete);
        }

        /// <summary>
        /// update an existing trainee
        /// </summary>
        /// <param name="updatedTrainee"></param>
        public void UpdateTrainee(Trainee updatedTrainee)
        {
            if (DataSource.Trainees.All(x => x.ID != updatedTrainee.ID))
                throw new Exception("Trainee doesn't exist");

            var trainee = DataSource.Trainees.Find(t => t.ID == updatedTrainee.ID);
            DataSource.Trainees.Remove(trainee);
            DataSource.Trainees.Add(updatedTrainee);
        }

        #endregion

        #region Return lists

        /// <summary>
        /// return a copy of all testers
        /// </summary>
        public IEnumerable<Tester> AllTesters => new List<Tester>(DataSource.Testers);

        /// <summary>
        /// return a copy of all tests
        /// </summary>
        public IEnumerable<Test> AllTests => new List<Test>(DataSource.Tests);

        /// <summary>
        /// return a copy of all trainees
        /// </summary>
        public IEnumerable<Trainee> AllTrainee => new List<Trainee>(DataSource.Trainees);

        #endregion
    }
}
