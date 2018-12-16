using System;
using System.Collections.Generic;
using BE.Routes;

namespace BE.MainObjects
{
    /// <summary>
    ///     A tester
    /// </summary>
    public class Tester : Person, ICloneable
    {
        private float _maxDistance;

        /// <summary>
        ///     An new tester
        /// </summary>
        public Tester()
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     A new Tester
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        /// <param name="gender">tester gender</param>
        public Tester(uint Id, string firstName = "", string lastName = "", Gender gender = Gender.Male) : base(Id,
            firstName, lastName, gender)
        {
            LicenseTypeTeaching = new List<LicenseType>();
            Schedule = new WeekSchedule((int) Configuration.NumbersOfWorkDaysInWeekTesters);
            Experience = 0;
            MaxWeekExams = 0;
            _maxDistance = 0;
        }

        /// <summary>
        ///     An new tester
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
        /// <param name="experience"></param>
        /// <param name="maxWeekExams"></param>
        /// <param name="licenseTypeTeaching"></param>
        /// <param name="maxDistance"></param>
        public Tester(uint Id, string firstName, string lastName, Gender gender, string emailAddress,
            DateTime birthDate, string phoneNumber, Address address, List<LicenseType> licenseTypes,
            uint experience, uint maxWeekExams, List<LicenseType> licenseTypeTeaching, float maxDistance) :
            base(Id, firstName, lastName, gender, emailAddress, birthDate, phoneNumber, address, licenseTypes)
        {
            Experience = experience;
            MaxWeekExams = maxWeekExams;
            LicenseTypeTeaching = licenseTypeTeaching;
            MaxDistance = maxDistance;
        }

        /// <summary>
        ///     Tester expirience
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
        ///     week schedule
        /// </summary>
        public WeekSchedule Schedule { set; get; }

        public object Clone()
        {
            var newLicnse = new List<LicenseType>();
            foreach (var item in LicenseType)
                newLicnse.Add(item);
            var newLicenseTypeTeaching = new List<LicenseType>();
            foreach (var item in LicenseTypeTeaching)
                newLicenseTypeTeaching.Add(item);
            var tester = new Tester
            {
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                BirthDate = BirthDate,
                LicenseType = newLicnse,
                Experience = Experience,
                MaxDistance = MaxDistance,
                LicenseTypeTeaching = newLicenseTypeTeaching,
                MaxWeekExams = MaxWeekExams
            };
            if (Id != 0) tester.Id = Id;
            if (PhoneNumber != null && PhoneNumber != "") tester.PhoneNumber = PhoneNumber;
            if (EmailAddress != null && EmailAddress != "") tester.EmailAddress = EmailAddress;
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
            return base.ToString() + " ,Job: A Tester ";
        }
    }
}