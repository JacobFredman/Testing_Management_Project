using System;
using System.Collections.Generic;

namespace BE
{
   public  class Trainee : Person
    {
        public List<LicenceType> LicenseTypeLearning { set; get; }
        public Gear GearType { set; get; }
        public string SchoolName { set; get; }
        public uint TesterId { set; get; }
        public uint NumberOfLessons { set; get; }
        public bool ReadyForTest { set; get; }

        public Trainee(uint id, string firstName, string lastName, Gender gender, string emailAddress, DateTime birthDate, string phoneNumber, Address address, List<LicenceType> licenseTypes,
          List<LicenceType> licenseTypesLearning, Gear gearType, string schoolName, uint testerId, uint numberOfLessons, bool readyForTest ) 
            : base(id, firstName, lastName, gender, emailAddress, birthDate, phoneNumber, address, licenseTypes)
        {
            LicenseTypeLearning = licenseTypesLearning;
            GearType = gearType;
            SchoolName = schoolName;
            TesterId = testerId;
            NumberOfLessons = numberOfLessons;
            ReadyForTest = readyForTest;
        }


        /// <inheritdoc />
        /// <summary>
        /// A new Trainee
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="gender">gender</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        public Trainee(uint id, Gender gender, string firstName = null, string lastName = null) : base(id, lastName, firstName, gender)
        {
            LicenseTypeLearning = new List<LicenceType>();
            this.GearType = Gear.Automat;
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
