using System.Collections.Generic;

namespace BE
{
   public  class Trainee : Person
    {
        private  Gender _gender;
        public List<LicenceType> LicenceTypeLearning { set; get; }
        public GearType GearType { set; get; }
        public string SchoolName { set; get; }
        public Tester TesterName { set; get; }
        public uint NumberOfLessons { set; get; }
        public bool ReadyForTest { set; get; }

        /// <inheritdoc />
        /// <summary>
        /// A new Trainee
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="gender">gender</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        public Trainee(uint id, Gender gender, string firstName = null, string lastName = null) :base(id, lastName, firstName, gender)
        {
            _gender = gender;
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
