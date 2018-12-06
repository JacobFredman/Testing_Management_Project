using System;
using System.Collections.Generic;
using BE.Routes;

namespace BE.MainObjects
{
   public  class Trainee : Person
    {
        public List<LicenseType> LicenseTypeLearning { set; get; }
        public Gear GearType { set; get; }
        public string SchoolName { set; get; }
        public string TesterId { set; get; }
        public uint NumberOfLessons { set; get; }
        public bool ReadyForTest { set; get; }

        public Trainee(uint Id, string firstName, string lastName, Gender gender, string emailAddress, DateTime birthDate, string phoneNumber, Address address, List<LicenseType> licenseTypes,
          List<LicenseType> licenseTypesLearning, Gear gearType, string schoolName, string testerId, uint numberOfLessons, bool readyForTest ) 
            : base(Id, firstName, lastName, gender, emailAddress, birthDate, phoneNumber, address, licenseTypes)
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
        /// <param name="Id">Id</param>
        /// <param name="gender">gender</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        public Trainee(uint Id, Gender gender, string firstName = null, string lastName = null) : base(Id, lastName, firstName, gender)
        {
            LicenseTypeLearning = new List<LicenseType>();
            this.GearType = Gear.Automatic;
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
