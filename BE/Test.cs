using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public class Test
    {
        
        public int Code { get; }
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
        public bool Pass { set; get; }
        public string Comment { set; get; }
        public int Id { get; set; }
         public LicenceType LicenceType { get; set; }

        public Test(int code,uint id_tester,uint id_trainee)
        {
            TesterId = id_tester;
            TraineeId = id_trainee;
            Code = code;
            Pass = false;
            Date = new DateTime();
            ActualDateTime = new DateTime();
            Address = new Address();
            Criterions = new List<Criterion>();
            Comment = "";
        }
        /// <summary>
        /// check the result according to the crterions
        /// </summary>
        public void CheckResults()
        {
            int i = 0;
            foreach(Criterion c in Criterions)
            {
                if (c.Pass)
                    i++;
            }
            if (i >= Configuration.MinimumCritirionstoPassTest)
                Pass = true;
        }
        public override string ToString()
        {

            return "Tester ID: " + TesterId + " Trainee ID: " + TraineeId + " Test Code: " + Code + " Pass Test: " + (Pass ? "yes" : "no");
        }
    }
}
