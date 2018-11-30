using System;
using System.Collections.Generic;

namespace BE
{
   public class Test
    {
        
        public uint Code { get; set; }
        private uint _testerId;
        public uint TesterId
        {
            get => _testerId; set
            {
                if (Tools.CheckID_IL(value))
                    _testerId = value;
                else
                    _testerId = 0;
            }
        }
        private uint _traineeId;
        public uint TraineeId
        {
            get => _traineeId; set
            {
                if (Tools.CheckID_IL(value))
                    _traineeId = value;
                else
                    _traineeId = 0;
            }
        }
        public DateTime Date { set; get; }
        public DateTime ActualDateTime { set; get; }
        public Address Address { set; get; }
        public List<Criterion> Criterions { set; get; }
        public bool Passed { set; get; }
        public string Comment { set; get; }
        public int Id { get; set; }
        public LicenceType LicenceType { get; set; }
        public Uri RouteUrl { set; get; }

        public Test(uint id_tester,uint id_trainee)
        {
            TesterId = id_tester;
            TraineeId = id_trainee;
            Code = 0;
            Passed = false;
            Date = new DateTime();
            ActualDateTime = DateTime.MinValue;
           
            Criterions = new List<Criterion>();
            Comment = "";
        }
        /// <summary>
        /// check the result according to the crterions
        /// </summary>
        
        public override string ToString()
        {

            return "Tester ID: " + TesterId + " Trainee ID: " + TraineeId + " Test Code: " + Code + " Passed Test: " + (Passed ? "yes" : "no");
        }
    }
}
