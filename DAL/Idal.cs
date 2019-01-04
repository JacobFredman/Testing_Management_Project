using System.Collections.Generic;
using BE.MainObjects;

namespace DAL
{
    /// <summary>
    /// </summary>
    public interface IDal
    {
        IEnumerable<Trainee> AllTrainee { get; }
        IEnumerable<Tester> AllTesters { get; }

        IEnumerable<Test> AllTests { get; }

        // Access to the Tester entity
        void AddTester(Tester newTester);
        void RemoveTester(Tester testerToDelete);
        void UpdateTester(Tester updatedTester);

        void AddTest(Test newTest);
        void RemoveTest(Test testToDelete);
        void UpdateTest(Test updatedTest);

        void AddTrainee(Trainee newTrainee);
        void RemoveTrainee(Trainee traineeToDelete);
        void UpdateTrainee(Trainee updatedTrainee);

        void SaveConfigurations();
        void LoadConfigurations();
    }
}