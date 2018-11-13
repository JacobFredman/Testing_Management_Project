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
        private uint testerID;
        public uint TesterID
        {
            get => testerID; set
            {
                if (Tools.CheckID_IL(value))
                    testerID = value;
                else
                    testerID = 0;
            }
        }
        private uint traineeID;
        public uint TraineeID
        {
            get => traineeID; set
            {
                if (Tools.CheckID_IL(value))
                    traineeID = value;
                else
                    traineeID = 0;
            }
        }
        public DateTime Date = new DateTime();
        public DateTime ActualDateTime= new DateTime();
        public Address Address = new Address();
        public List<Criterion> Criterions = new List<Criterion>();
        public bool Pass { set; get; }
        public string Comment { set; get; }
        public Test(int code,uint id_tester,uint id_trainee)
        {
            TesterID = id_tester;
            TraineeID = id_trainee;
            Code = code;
            Pass = false;
        }
        /// <summary>
        /// check the resoult according to the criterions
        /// </summary>
        public void CheckResoults()
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

            return "Tester ID: " + TesterID + " Trainee ID: " + TraineeID + " Test Code: " + Code + " Pass Test: " + (Pass ? "yes" : "no");
        }
    }
}
