using System;
using System.Collections.Generic;
using System.Linq;
using BE.Routes;

namespace BE.MainObjects
{
    /// <summary>
    ///     A vehicle test
    /// </summary>
    public class Test : ICloneable
    {
        /// <summary>
        ///     tester id
        /// </summary>
        private uint _testerId;

        /// <summary>
        ///     trainee id
        /// </summary>
        private uint _traineeId;

        public Test(uint idTester, uint idTrainee)
        {
            TesterId = idTester;
            TraineeId = idTrainee;
            Id = "";
            TestTime = new DateTime();
            ActualTestTime = DateTime.MinValue;
            Criteria = new List<Criterion>();
            Comment = "";
        }

        /// <summary>
        ///     For debbuging
        /// </summary>
        /// <param name="testerId"></param>
        /// <param name="traineeId"></param>
        /// <param name="testTime"></param>
        /// <param name="addressOfBeginningTest"></param>
        /// <param name="criteria"></param>
        /// <param name="passed"></param>
        /// <param name="Id"></param>
        /// <param name="licenseType"></param>
        public Test(uint testerId, uint traineeId, DateTime testTime, Address addressOfBeginningTest,
            List<Criterion> criteria, bool passed, string Id, LicenseType licenseType)
        {
            _testerId = testerId;
            _traineeId = traineeId;
            TestTime = testTime;
            AddressOfBeginningTest = addressOfBeginningTest;
            Criteria = criteria;
            Passed = passed;
            this.Id = Id;
            LicenseType = licenseType;
        }

        public string Id { get; set; }

        public uint TesterId
        {
            get => _testerId;
            set => _testerId = Tools.CheckID_IL(value) ? value : 0;
        }

        public uint TraineeId
        {
            get => _traineeId;
            set => _traineeId = Tools.CheckID_IL(value) ? value : 0;
        }

        /// <summary>
        ///     planned test time
        /// </summary>
        public DateTime TestTime { set; get; }

        /// <summary>
        ///     time that the test was
        /// </summary>
        public DateTime ActualTestTime { set; get; }

        public Address AddressOfBeginningTest { set; get; }

        /// <summary>
        ///     list of criterions
        /// </summary>
        public List<Criterion> Criteria { set; get; }

        /// <summary>
        ///     if passed the test
        /// </summary>
        public bool? Passed { set; get; }

        /// <summary>
        ///     a comment
        /// </summary>
        public string Comment { set; get; }

        /// <summary>
        ///     license type test
        /// </summary>
        public LicenseType LicenseType { get; set; }

        /// <summary>
        ///     url for the test route on google maps
        /// </summary>
        public Uri RouteUrl { set; get; }

        public object Clone()
        {
            var newCriteria = new List<Criterion>();
            foreach (var item in Criteria)
                newCriteria.Add(item.Clone() as Criterion);
            return new Test(TesterId, TraineeId)
            {
                TestTime = TestTime,
                ActualTestTime = ActualTestTime,
                AddressOfBeginningTest =
                    AddressOfBeginningTest != null ? AddressOfBeginningTest.Clone() as Address : null,
                Criteria = newCriteria,
                Passed = Passed,
                Id = Id,
                LicenseType = LicenseType,
                RouteUrl = RouteUrl != null ? RouteUrl : null
            };
        }

        /// <summary>
        ///     update test results
        /// </summary>
        public void UpdatePassedTest()
        {
            Passed = Criteria.Count(x => x.Pass) / (double) Criteria.Count() * 100 >
                     Configuration.PercentOfCritirionsToPassTest;
        }

        public override string ToString()
        {
            return "Tester Id: " + TesterId + " Trainee Id: " + TraineeId + " Test Code: " + Id + " Passed Test: " +
                   (Passed == true ? "yes" : "no");
        }
    }
}