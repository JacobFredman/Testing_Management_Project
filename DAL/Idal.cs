using System.Collections.Generic;
using BE;

namespace DAL
{
    internal interface IDal
    {
        void AddTester(Tester newTester);
        void RemoveTester(Tester testerToDelete);
        void UpdateTester(Tester updatedTester);

        void AddTest(Test newTest);
        void RemoveTest(Test testToDelete);
        void UpdateTest(Test updatedTest);

        void AddTrainee(Trainee newTrainee);
        void RemoveTrainee(Trainee traineeToDelete);
        void UpdateTrainee(Trainee updatedTrainee);

        IEnumerable<Trainee> AllTrainee { get; }
        IEnumerable<Tester> AllTesters { get; }
        IEnumerable<Test> AllTests { get; }
    }
}
