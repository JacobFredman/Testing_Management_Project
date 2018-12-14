using System;
using System.Collections.Generic;
using BE.Routes;

namespace BE.MainObjects
{
    /// <summary>
    /// Licnese lessons
    /// </summary>
    public class LessonsAndType
    {
        private int _numberOfLessons;

        /// <summary>
        /// license
        /// </summary>
        public LicenseType License { set; get; }

        /// <summary>
        /// number of lessons
        /// </summary>
        public int NumberOfLessons { get => _numberOfLessons; set { _numberOfLessons = value; ReadyForTest = NumberOfLessons > Configuration.MinLessons; } }

        //ready for test
        public bool ReadyForTest { set; get; }

        /// <summary>
        /// lesson details
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "License type: "+License.ToString()+", Num of lessons: "+NumberOfLessons+", ready for test: "+((ReadyForTest)?"yes":"no");
        }
    }

    /// <summary>
    /// A person
    /// </summary>
    public  class Trainee : Person
    {
        /// <summary>
        /// License type learning
        /// </summary>
        public List<LessonsAndType> LicenseTypeLearning { set; get; }

        /// <summary>
        /// gear type
        /// </summary>
        public Gear GearType { set; get; }

        /// <summary>
        /// School name
        /// </summary>
        public string SchoolName { set; get; }

        /// <summary>
        /// last Tester id
        /// </summary>
        public string TesterId { set; get; }

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
            LicenseTypeLearning = new List<LessonsAndType>();
            this.GearType = Gear.Automatic;
            SchoolName = "";
        }

        /// <summary>
        /// A new trainee
        /// </summary>
        public Trainee() { }

        /// <summary>
        /// Trainee details
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + " ,Job: Trainee";
        }

        /// <summary>
        /// A new trainee
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="gender"></param>
        /// <param name="emailAddress"></param>
        /// <param name="birthDate"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="address"></param>
        /// <param name="licenseTypes"></param>
        /// <param name="licenseTypesLearning"></param>
        /// <param name="gearType"></param>
        /// <param name="schoolName"></param>
        /// <param name="testerId"></param>
        /// <param name="numberOfLessons"></param>
        /// <param name="readyForTest"></param>
        public Trainee(uint Id, string firstName, string lastName, Gender gender, string emailAddress, DateTime birthDate, string phoneNumber, Address address, List<LicenseType> licenseTypes,
          List<LessonsAndType> licenseTypesLearning, Gear gearType, string schoolName, string testerId, uint numberOfLessons, bool readyForTest)
            : base(Id, firstName, lastName, gender, emailAddress, birthDate, phoneNumber, address, licenseTypes)
        {
            LicenseTypeLearning = licenseTypesLearning;
            GearType = gearType;
            SchoolName = schoolName;
            TesterId = testerId;
        }
    }
}
