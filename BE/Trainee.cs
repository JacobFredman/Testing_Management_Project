using System.Collections.Generic;

namespace BE
{
   public  class Trainee : Person
    {
        /// <summary>
        /// License type
        /// </summary>
        public List<LicenseType> LicenseTypeLearning { set; get; }

        /// <summary>
        /// Gear type learning
        /// </summary>
        public Gear GearType { set; get; }

        /// <summary>
        /// school name
        /// </summary>
        public string SchoolName { set; get; }

        /// <summary>
        /// tester name
        /// </summary>
        public Tester TesterName { set; get; }

        /// <summary>
        /// number of lessons
        /// </summary>
        public uint NumberOfLessons { set; get; }

        /// <summary>
        /// is ready for test
        /// </summary>
        public bool ReadyForTest { set; get; }

        /// <inheritdoc />
        /// <summary>
        /// A new Trainee
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="gender">gender</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        public Trainee(string id, string firstName = null, string lastName = null, Gender gender=Gender.Male) :base(id, lastName, firstName, gender)
        {
            LicenseTypeLearning = new List<LicenseType>();
            GearType = Gear.Automatic;
            SchoolName = "";
            NumberOfLessons = 0;
            ReadyForTest = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// information about trainee
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + " ,Job: Trainee";
        }
    }
}
