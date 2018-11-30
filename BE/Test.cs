using System;
using System.Collections.Generic;

namespace BE
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
        public bool Passed { set; get; }
        public string Comment { set; get; }
        public uint Id { get; set; }
        public LicenceType LicenseType { get; set; }
        public Uri RouteUrl { set; get; }

        public Test(uint idTester,uint idTrainee)
        {
            TesterId = idTester;
            TraineeId = idTrainee;
            Id = 0;
            Passed = false;
            TestTime = new DateTime();
            ActualTestTime = DateTime.MinValue;
           
            Criteria = new List<Criterion>();
            Comment = "";
        }

        /// <summary>
        /// check the result according to the Criteria
        /// </summary>

        public Test(uint testerId, uint traineeId,DateTime testTime,Address addressOfBeginningTest)
        {

        }



        public override string ToString()
        {

            return "Tester ID: " + TesterId + " Trainee ID: " + TraineeId + " Test Code: " + Id + " Passed Test: " + (Passed ? "yes" : "no");
        }
    }
}
