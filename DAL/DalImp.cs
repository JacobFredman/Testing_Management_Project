using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using BE.MainObjects;
using DS;

namespace DAL
{
    /// <summary>
    ///     Get Instance of DAL
    /// </summary>
    public static class FactoryDal
    {
        private static DalImp _dal;

        /// <summary>
        ///     Get the object
        /// </summary>
        public static DalImp GetObject => _dal ?? (_dal = new DalImp());
    }

    /// <inheritdoc />
    /// <summary>
    ///     DAL implantation
    /// </summary>
    public class DalImp : IDal
    {
        /// <summary>
        ///     denay access to c-tor
        /// </summary>
        internal DalImp()
        {
        }

        #region Test

        /// <summary>
        ///     Add a new test
        /// </summary>
        /// <param name="newTest"></param>
        public void AddTest(Test newTest)
        {
            newTest.Id = $"{Configuration.TestId:00000000}";
            Configuration.TestId++;

            DataSource.Tests.Add(newTest);
        }

        /// <summary>
        ///     remove a test
        /// </summary>
        /// <param name="testToDelete"></param>
        public void RemoveTest(Test testToDelete)
        {
            if (DataSource.Tests.All(x => x.Id != testToDelete.Id))
                throw new Exception("Test doesn't exist");

            DataSource.Tests.RemoveAll(x => x.Id == testToDelete.Id);
        }

        /// <summary>
        ///     update an existing test
        /// </summary>
        /// <param name="updatedTest"></param>
        public void UpdateTest(Test updatedTest)
        {
            if (DataSource.Tests.All(x => x.Id != updatedTest.Id))
                throw new Exception("Test doesn't exist");

            var test = DataSource.Tests.Find(t => t.Id == updatedTest.Id);
            DataSource.Tests.Remove(test);
            DataSource.Tests.Add(updatedTest);
        }

        #endregion

        #region Tester

        /// <summary>
        ///     Add tester
        /// </summary>
        /// <param name="newTester"></param>
        public void AddTester(Tester newTester)
        {
            if (DataSource.Testers.Any(tester => tester.Id == newTester.Id))
                throw new Exception("the tester already exist in the system");

            DataSource.Testers.Add(newTester);
        }

        /// <summary>
        ///     Remove a tester
        /// </summary>
        /// <param name="testerToDelete"></param>
        public void RemoveTester(Tester testerToDelete)
        {
            if (DataSource.Testers.All(x => x.Id != testerToDelete.Id))
                throw new Exception("Tester doesn't exist");

            DataSource.Testers.RemoveAll(x => x.Id == testerToDelete.Id);
        }

        /// <summary>
        ///     update existing Tester
        /// </summary>
        /// <param name="updatedTester"></param>
        public void UpdateTester(Tester updatedTester)
        {
            if (DataSource.Testers.All(x => x.Id != updatedTester.Id))
                throw new Exception("Tester doesn't exist");

            var tester = DataSource.Testers.Find(t => t.Id == updatedTester.Id);
            DataSource.Testers.Remove(tester);
            DataSource.Testers.Add(updatedTester);
        }

        #endregion

        #region Trainee

        /// <summary>
        ///     Add trainee
        /// </summary>
        /// <param name="newTrainee"></param>
        public void AddTrainee(Trainee newTrainee)
        {
            if (DataSource.Trainees.Any(t => t.Id == newTrainee.Id))
                throw new Exception("the trainee already exist in the system");

            DataSource.Trainees.Add(newTrainee);
        }

        /// <summary>
        ///     remove trainee
        /// </summary>
        /// <param name="traineeToDelete"></param>
        public void RemoveTrainee(Trainee traineeToDelete)
        {
            if (DataSource.Trainees.All(x => x.Id != traineeToDelete.Id))
                throw new Exception("Trainee doesn't exist");


            DataSource.Trainees.RemoveAll(x => x.Id == traineeToDelete.Id);
        }

        /// <summary>
        ///     update an existing trainee
        /// </summary>
        /// <param name="updatedTrainee"></param>
        public void UpdateTrainee(Trainee updatedTrainee)
        {
            if (DataSource.Trainees.All(x => x.Id != updatedTrainee.Id))
                throw new Exception("Trainee doesn't exist");

            var trainee = DataSource.Trainees.Find(t => t.Id == updatedTrainee.Id);
            DataSource.Trainees.Remove(trainee);
            DataSource.Trainees.Add(updatedTrainee);
        }

        #endregion

        #region Return lists

        /// <summary>
        ///     return a copy of all testers
        /// </summary>
        public IEnumerable<Tester> AllTesters
        {
            get
            {
                var allTesters = new List<Tester>();
                foreach (var item in DataSource.Testers)
                    allTesters.Add(item.Clone() as Tester);
                return allTesters.OrderBy(x=>x.Id);
            }
        }

        /// <summary>
        ///     return a copy of all tests
        /// </summary>
        public IEnumerable<Test> AllTests
        {
            get
            {
                var allTest = new List<Test>();
                foreach (var item in DataSource.Tests)
                    allTest.Add(item.Clone() as Test);
                return allTest.OrderBy(x => x.Id); ;
            }
        }

        /// <summary>
        ///     return a copy of all trainees
        /// </summary>
        public IEnumerable<Trainee> AllTrainee
        {
            get
            {
                var allTrainee = new List<Trainee>();
                foreach (var item in DataSource.Trainees)
                    allTrainee.Add(item.Clone() as Trainee);
                return allTrainee.OrderBy(x => x.Id); ;
            }
        }

        #endregion
    }
}