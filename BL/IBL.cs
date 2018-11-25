﻿using System.Collections.Generic;
using BE;

namespace BL
{
    internal interface IBL
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

        List<Trainee> AllTrainee { get; }
        List<Tester> AllTesters { get; }
        List<Test> AllTests { get; }
    }
}