using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using BE.MainObjects;
using BE.Routes;

namespace BL
{
    public interface IBl
    {
        //get list copy's
        IEnumerable<Trainee> AllTrainees { get; }
        IEnumerable<Tester> AllTesters { get; }
        IEnumerable<Test> AllTests { get; }

        //access testers
        void AddTester(Tester newTester);
        void RemoveTester(Tester testerToDelete);
        void UpdateTester(Tester updatedTester);

        //access tests
        void AddTest(Test newTest);
        void RemoveTest(Test testToDelete);
        void UpdateTest(Test updatedTest);

        //access trainees
        void AddTrainee(Trainee newTrainee);
        void RemoveTrainee(Trainee traineeToDelete);
        void UpdateTrainee(Trainee updatedTrainee);

        //get list of testers that..
        IEnumerable<Tester> GetAvailableTesters(DateTime date);
        IEnumerable<Tester> GetAllTestersInRadios(int r, Address a);
        IEnumerable<Tester> GetTestersByDistance(Address address, LicenseType license);

        //get tests that..
        IEnumerable<Test> GetAllTestsSortedByDate();
        IEnumerable<Test> GetAllTestsWhere(Func<Test, bool> func);
        IEnumerable<Test> GetAllTestInMonth(DateTime date);
        IEnumerable<Test> GetAllTestInDay(DateTime date);
        IEnumerable<Test> GetAllTestsToCome();
        IEnumerable<Test> GetAllTestsThatHappened();
        IEnumerable<Trainee> GetAllTraineeThatPassedToday(DateTime date);
        IEnumerable<Trainee> GetAllTraineeThatDidNotPassedToday(DateTime date);


        //get groups
        IEnumerable<IGrouping<LicenseType, Test>> GetAllTestsByLicense(bool sorted = false);
        IEnumerable<IGrouping<List<LicenseType>, Trainee>> GetAllTraineesByLicense(bool sorted = false);
        IEnumerable<IGrouping<List<LicenseType>, Tester>> GetAllTestersByLicense(bool sorted = false);
        IEnumerable<IGrouping<string, Trainee>> GetAllTraineesByTester(bool sorted = false);
        IEnumerable<IGrouping<string, Trainee>> GetAllTraineesBySchool(bool sorted = false);
        IEnumerable<IGrouping<int, Trainee>> GetAllTraineeByNumberOfTests(bool sorted = false);

        int GetNumberOfTests(Trainee trainee);
        bool TraineePassedTest(Trainee trainee, LicenseType license);
        string GetTypeFromId(int id, DateTime birthDate);

        //Search functions
        IEnumerable<Trainee> SearchTrainee(string key);
        IEnumerable<Tester> SearchTester(string key);
        IEnumerable<Test> SearchTest(string key);

        void SaveSettings();
    }
}