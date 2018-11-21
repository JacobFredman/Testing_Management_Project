using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

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

        IEnumerator<Trainee> GetAllTrainee();
        IEnumerator<Tester> GetAllTesters();
        IEnumerator<Test> GetAllTests();
    }
}
