using System;
using System.Collections.Generic;
using BE.Routes;

namespace BE.MainObjects
{
    /// <inheritdoc />
    /// <summary>
    ///     A person
    /// </summary>
    public class Trainee : Person, ICloneable
    {
        /// <summary>
        ///     License type learning
        /// </summary>
        public List<TrainingDetails> LicenseTypeLearning { set; get; }

        /// <summary>
        ///     School name
        /// </summary>
        public string SchoolName { set; get; }

        /// <summary>
        ///     last Tester id
        /// </summary>
        public string TeacherName { set; get; }

        /// <summary>
        ///     Clone trainee
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var newLicense = new List<LicenseType>();
            if (LicenseType != null)
                foreach (var item in LicenseType)
                    newLicense.Add(item);
            else newLicense = null;
            var newLicenseTypeLearning = new List<TrainingDetails>();
            if (LicenseTypeLearning != null)
                foreach (var item in LicenseTypeLearning)
                    newLicenseTypeLearning.Add(item.Clone() as TrainingDetails);
            else newLicenseTypeLearning = null;
            var trainee = new Trainee
            {
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                BirthDate = BirthDate,
                LicenseType = newLicense,
                LicenseTypeLearning = newLicenseTypeLearning,
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
            return base.ToString() + " Trainee";
        }
    }
}