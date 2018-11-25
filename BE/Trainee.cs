using System.Collections.Generic;

namespace BE
{
   public  class Trainee : Person
    {
        public List<LicenceType> LicenceTypeLearning { set; get; }
        public GearType GearType { set; get; }
        public string SchoolName { set; get; }
        public Tester TesterName { set; get; }
        public uint NumberOfLessons { set; get; }
        public bool ReadyForTest { set; get; }
        /// <summary>
        /// A new Trainee
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="Fn">first name</param>
        /// <param name="Ln">last name</param>
        public Trainee(uint id, string Fn = null, string Ln = null) :base(id,Fn,Ln){
            LicenceTypeLearning = new List<LicenceType>();
            this.GearType = GearType.Automatic;
            SchoolName = "";
            NumberOfLessons = 0;
            ReadyForTest = false;
        }
        public override string ToString()
        {
            return base.ToString() + " ,Job: Trainee";
        }
    }
}
