using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    interface IDal
    {
        void AddTester(Tester t);
        void RemoveTester(Tester t);
        void UpdateTester(Tester t);

        void AddTest(Test t);
        void RemoveTest(Test t);
        void UpdateTest(Test t);

        void AddTrainee(Trainee t);
        void RemoveTrainee(Trainee t);
        void UpdateTrainee(Trainee t);

        IEnumerator<Trainee> GetAllTrainee();
        IEnumerator<Tester> GetAllTesters();
        IEnumerator<Test> GetAllTests();
    }
}
