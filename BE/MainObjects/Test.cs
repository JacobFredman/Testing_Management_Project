using System;
using System.Collections.Generic;
using System.Linq;
using BE.Routes;

namespace BE.MainObjects
{
   public class Test
    {
        
        private uint _testerId;
        public uint TesterId
        {
            get => _testerId;
            set => _testerId = Tools.CheckID_IL(value) ? value : 0;
        }

        private uint _traineeId;
        public uint TraineeId
        {
            get => _traineeId;
            set => _traineeId = Tools.CheckID_IL(value) ? value : 0;
        }

        public DateTime TestTime { set; get; }
        public DateTime ActualTestTime { set; get; }
        public Address AddressOfBeginningTest { set; get; }
        public List<Criterion> Criteria { set; get; }
        public bool? Passed { set; get; }
        public string Comment { set; get; }
        public string Id { get; set; }
        public LicenseType LicenseType { get; set; }
        public Uri RouteUrl { set; get; }

        public Test(uint idTester,uint idTrainee)
        {
            TesterId = idTester;
            TraineeId = idTrainee;
            Id = "";
            Passed = false;
            TestTime = new DateTime();
            ActualTestTime = DateTime.MinValue;
           
            Criteria = new List<Criterion>();
            Comment = "";
        }

        /// <summary>
        /// check the result according to the Criteria
        /// </summary>

        public Test(uint testerId, uint traineeId,DateTime testTime,Address addressOfBeginningTest,
            List<Criterion> criteria,bool passed,string Id, LicenseType licenseType)
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

        public void UpdatePassedTest()
        {
            Passed = Criteria.Count(x => x.Pass) / Criteria.Count() > Configuration.PercentOfCritirionsToPassTest;
        }

        public override string ToString()
        {

            return "Tester Id: " + TesterId + " Trainee Id: " + TraineeId + " Test Code: " + Id + " Passed Test: " + (Passed ==true ? "yes" : "no");
        }
    }
}
