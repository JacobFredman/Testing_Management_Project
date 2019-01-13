using System;
using System.Collections.Generic;
using BE.Routes;

namespace BE.MainObjects
{
    /// <inheritdoc />
    /// <summary>
    ///     A tester
    /// </summary>
    public class Tester : Person, ICloneable
    {
        private float _maxDistance;

        /// <summary>
        ///     Max distance
        /// </summary>
        public float MaxDistance
        {
            get => _maxDistance;
            set
            {
                if (value >= 0) _maxDistance = value;
            }
        }

        /// <summary>
        ///     Tester experience
        /// </summary>
        public uint Experience { get; set; }

        /// <summary>
        ///     Max exams in week
        /// </summary>
        public uint MaxWeekExams { set; get; }

        /// <summary>
        ///     License type teaching
        /// </summary>
        public List<LicenseType> LicenseTypeTeaching { set; get; }

       

        /// <summary>
        ///     week schedule
        /// </summary>
        public WeekSchedule Schedule { set; get; }

        public object Clone()
        {
            var newLicense = new List<LicenseType>();
            if (LicenseType != null)
                foreach (var item in LicenseType)
                    newLicense.Add(item);
            else newLicense = null;

            var newLicenseTypeTeaching = new List<LicenseType>();
            foreach (var item in LicenseTypeTeaching)
                newLicenseTypeTeaching.Add(item);
            var tester = new Tester
            {
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                BirthDate = BirthDate,
                LicenseType = newLicense,
                Experience = Experience,
                MaxDistance = MaxDistance,
                LicenseTypeTeaching = newLicenseTypeTeaching,
                MaxWeekExams = MaxWeekExams
            };
            if (Id != 0) tester.Id = Id;
            if (!string.IsNullOrEmpty(PhoneNumber)) tester.PhoneNumber = PhoneNumber;
            if (!string.IsNullOrEmpty(EmailAddress)) tester.EmailAddress = EmailAddress;
            if (Address != null) tester.Address = Address.Clone() as Address;
            if (Schedule != null) tester.Schedule = Schedule.Clone() as WeekSchedule;
            return tester;
        }


        /// <summary>
        ///     Basic data about tester
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + " , Tester ";
        }
    }
}