using System;
using System.Collections.Generic;
using BE.Routes;

namespace BE.MainObjects
{
    /// <summary>
    ///     A person
    /// </summary>
    public class Trainee : Person, ICloneable
    {
        /// <inheritdoc />
        /// <summary>
        ///     A new Trainee
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="gender">gender</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        public Trainee(uint Id, Gender gender, string firstName = null, string lastName = null) : base(Id, lastName,
            firstName, gender)
        {
            LicenseTypeLearning = new List<LessonsAndType>();
            GearType = Gear.Automatic;
            SchoolName = "";
        }

        /// <summary>
        ///     A new trainee
        /// </summary>
        public Trainee()
        {
        }

        /// <summary>
        ///     A new trainee
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
        /// <param name="teacherName"></param>
        /// <param name="numberOfLessons"></param>
        /// <param name="readyForTest"></param>
        public Trainee(uint Id, string firstName, string lastName, Gender gender, string emailAddress,
            DateTime birthDate, string phoneNumber, Address address, List<LicenseType> licenseTypes,
            List<LessonsAndType> licenseTypesLearning, Gear gearType, string schoolName, string teacherName,
            uint numberOfLessons, bool readyForTest)
            : base(Id, firstName, lastName, gender, emailAddress, birthDate, phoneNumber, address, licenseTypes)
        {
            LicenseTypeLearning = licenseTypesLearning;
            GearType = gearType;
            SchoolName = schoolName;
            TeacherName = teacherName;
        }

        /// <summary>
        ///     License type learning
        /// </summary>
        public List<LessonsAndType> LicenseTypeLearning { set; get; }

        /// <summary>
        ///     gear type
        /// </summary>
        public Gear GearType { set; get; }

        /// <summary>
        ///     School name
        /// </summary>
        public string SchoolName { set; get; }

        /// <summary>
        ///     last Tester id
        /// </summary>
        public string TeacherName { set; get; }

        public object Clone()
        {
            var newLicense = new List<LicenseType>();
            if (LicenseType != null)
                foreach (var item in LicenseType)
                    newLicense.Add(item);
            else newLicense = null;
            var newLicenseTypeLearning = new List<LessonsAndType>();
            foreach (var item in LicenseTypeLearning)
                newLicenseTypeLearning.Add(item.Clone() as LessonsAndType);
            var trainee = new Trainee
            {
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                BirthDate = BirthDate,
                LicenseType = newLicense,
                LicenseTypeLearning = newLicenseTypeLearning,
                GearType = GearType,
                SchoolName = SchoolName,
                TeacherName = TeacherName
            };
            if (Address != null) trainee.Address = Address.Clone() as Address;
            if (Id != 0) trainee.Id = Id;
            if (!string.IsNullOrEmpty(PhoneNumber)) trainee.PhoneNumber = PhoneNumber;
            if (!string.IsNullOrEmpty(EmailAddress)) trainee.EmailAddress = EmailAddress;
            return trainee;
        }

        /// <summary>
        ///     Trainee details
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + " ,Job: Trainee";
        }
    }
}