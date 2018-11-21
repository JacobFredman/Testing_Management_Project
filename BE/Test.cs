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
        public DateTime Date { set; get; }
        public DateTime ActualDateTime { set; get; }
        public Address Address { set; get; }
        public List<Criterion> Criterions { set; get; }
        public bool Pass { set; get; }
        public string Comment { set; get; }
        public int Id { get; set; }

        public Test(int code,uint id_tester,uint id_trainee)
        {
            TesterID = id_tester;
            TraineeID = id_trainee;
            Code = code;
            Pass = false;
            Date = new DateTime();
            ActualDateTime = new DateTime();
            Address = new Address();
            Criterions = new List<Criterion>();
            Comment = "";
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
