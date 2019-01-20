using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using BE.Routes;

namespace BE.MainObjects
{
    /// <inheritdoc />
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

        /// <summary>
        /// Test
        /// </summary>
        public Test()
        {
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <param name="idTester">Tester id</param>
        /// <param name="idTrainee">Trainee id</param>
        private Test(uint idTester, uint idTrainee)
        {
            TesterId = idTester;
            TraineeId = idTrainee;
            Id = "";
            TestTime = new DateTime();
            ActualTestTime = DateTime.MinValue;
            Criteria = new List<Criterion>();
            Comment = "";
        }


        public string Id { get; set; }

        /// <summary>
        /// Tester id
        /// </summary>
        public uint TesterId
        {
            get => _testerId;
            set => _testerId = Tools.CheckID_IL(value) ? value : 0;
        }

        /// <summary>
        /// Trainee id
        /// </summary>
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

        /// <summary>
        /// Address of the beginning  of the test
        /// </summary>
        public Address AddressOfBeginningTest { set; get; }

        /// <summary>
        ///     list of criteria
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
        public LicenseType? LicenseType { get; set; }

        /// <summary>
        ///     url for the test route on google maps
        /// </summary>
        [XmlIgnore]
        public Uri RouteUrl { set; get; }

        /// <summary>
        /// For xml serializer
        /// </summary>
        public string XmlSaveRouteUrlSerializer
        {
            get => RouteUrl?.ToString();
            set => RouteUrl = new Uri(value);
        }

        /// <summary>
        /// Clone test
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var newCriteria = new List<Criterion>();
            if (Criteria != null)
                foreach (var item in Criteria)
                    newCriteria.Add(item.Clone() as Criterion);
            else newCriteria = null;

            return new Test(TesterId, TraineeId)
            {
                TestTime = TestTime,
                ActualTestTime = ActualTestTime,
                AddressOfBeginningTest =
                    AddressOfBeginningTest?.Clone() as Address,
                Criteria = newCriteria,
                Passed = Passed,
                Comment = Comment,
                Id = Id,
                LicenseType = LicenseType,
                RouteUrl = RouteUrl ?? null
            };
        }

        /// <summary>
        ///     update test results
        /// </summary>
        public void UpdatePassedTest()
        {
            Passed = Criteria.Count(x => x.Pass) / (double) Criteria.Count * 100 >
                     Configuration.PercentOfCriteriaToPassTest;
        }

        /// <summary>
        /// test details
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Tester Id: " + TesterId + " Trainee Id: " + TraineeId + " Test Code: " + Id + " Passed Test: " +
                   (Passed == true ? "yes" : "no");
        }
    }
}