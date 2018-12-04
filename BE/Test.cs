using System;
using System.Collections.Generic;

namespace BE
{
   public class Test
    {
        
        private string _testerId;
        public string TesterId
        {
            get => _testerId; set
            {
                if (Tools.CheckID_IL(uint.Parse(value)))
                    _testerId = string.Format("{0:000000000}", uint.Parse(value));
                else
                    throw new Exception("Invalied tester id");
            }
        }
        private string _traineeId;
        public string TraineeId
        {
            get => _traineeId; set
            {
                if (Tools.CheckID_IL(uint.Parse(value)))
                    _traineeId = string.Format("{0:000000000}", uint.Parse(value));
                else
                    throw new Exception("Invalied tester id");
            }
        }
        public DateTime Date { set; get; }
        public DateTime ActualDateTime { set; get; }
        public Address Address { set; get; }
        public List<Criterion> Criterions { set; get; }
        public bool Passed { set; get; }
        public string Comment { set; get; }
        public string ID { get; set; }
        public LicenceType LicenceType { get; set; }
        public Uri RouteUrl { set; get; }

        public Test(string id_tester,string id_trainee)
        {
            TesterId = id_tester;
            TraineeId = id_trainee;
            ID = "00000000";
            Passed = false;
            ActualDateTime = DateTime.MinValue;
           
            Criterions = new List<Criterion>();
            Comment = "";
        }
        /// <summary>
        /// check the result according to the crterions
        /// </summary>
        
        public override string ToString()
        {

            return "Tester ID: " + TesterId + " Trainee ID: " + TraineeId + " Test Code: " + ID + " Passed Test: " + (Passed ? "yes" : "no");
        }
    }
}
