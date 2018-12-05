using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    /// <summary>
    /// An vehicle test
    /// </summary>
   public class Test
    {
        
        private string _testerId;

        /// <summary>
        /// tester id
        /// </summary>
        public string TesterId
        {
            get => _testerId; set
            {
                if (Tools.CheckID_IL(uint.Parse(value)))
                    _testerId = $"{uint.Parse(value):000000000}";
                else
                    throw new Exception("Invalid tester id");
            }
        }

        private string _traineeId;

        /// <summary>
        /// Trainee id
        /// </summary>
        public string TraineeId
        {
            get => _traineeId; set
            {
                if (Tools.CheckID_IL(uint.Parse(value)))
                    _traineeId = $"{uint.Parse(value):000000000}";
                else
                    throw new Exception("Invalid tester id");
            }
        }

        /// <summary>
        /// Date to set the test
        /// </summary>
        public DateTime Date { set; get; }

        /// <summary>
        /// When the test was
        /// </summary>
        public DateTime ActualDateTime { set; get; }

        /// <summary>
        /// test start address
        /// </summary>
        public Address Address { set; get; }

        /// <summary>
        /// A list with the criterions
        /// </summary>
        public List<Criterion> Criterions { set; get; }

        /// <summary>
        /// If passed the test
        /// </summary>
        public bool? Passed { set; get; }

        /// <summary>
        /// A comment about the test
        /// </summary>
        public string Comment { set; get; }

        /// <summary>
        /// test id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// license type
        /// </summary>
        public LicenseType LicenseType { get; set; }

        /// <summary>
        /// url to show the route on a map
        /// </summary>
        public Uri RouteUrl { set; get; }

        /// <summary>
        /// A new test
        /// </summary>
        /// <param name="id_tester">Tester id</param>
        /// <param name="id_trainee">Trainee id</param>
        public Test(string id_tester,string id_trainee)
        {
            TesterId = id_tester;
            TraineeId = id_trainee;
            ID = "00000000";
            Passed = null;
            ActualDateTime = DateTime.MinValue;
           
            Criterions = new List<Criterion>();
            Comment = "";
        }

        /// <summary>
        /// Update the test if the Trainee Passed according to the criterion
        /// </summary>
        /// <param name="test">The Test</param>
        public void UpdatePassedTest()
        {
            var percent = Criterions.Count(x => x.Pass) / (double)Criterions.Count;
            Passed = (percent >= Configuration.PercentOfCritirionsToPassTest);
        }

        /// <summary>
        /// Information about the test
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            return "Tester ID: " + TesterId + " Trainee ID: " + TraineeId + " Test Code: " + ID + " Passed Test: " + (Passed==true ? "yes" : "no");
        }
    }
}
