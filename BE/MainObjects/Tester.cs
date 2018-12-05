using System;
using System.Collections.Generic;

namespace BE.MainObjects
{
   
   public class Tester:Person
    {
        public Tester(uint id, string firstName, string lastName, Gender gender, string emailAddress, DateTime birthDate, string phoneNumber, Address address, List<LicenceType> licenseTypes,
                    uint experience, uint maxWeekExams, List<LicenceType> licenseTypeTeaching, float maxDistance   ) :
                base(id, firstName, lastName, gender, emailAddress, birthDate, phoneNumber, address, licenseTypes)
        {
            Experience = experience;
            MaxWeekExams = maxWeekExams;
            LicenseTypeTeaching = licenseTypeTeaching;
            MaxDistance = maxDistance;
        }

        public uint Experience { get; set ; }
        public uint MaxWeekExams { set; get; }
        public List<LicenceType> LicenseTypeTeaching { set; get; }
        private float _maxDistance;

        public float MaxDistance
        {
            get => _maxDistance;
            set { if (value >= 0) _maxDistance = value; }
        }

        public WeekSchedule Schedule { set; get; }

        /// <inheritdoc />
        /// <summary>
        /// A new Tester
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        /// <param name="gender">tester gender</param>
        public Tester(uint id, string firstName = "", string lastName = "", Gender gender = Gender.Male) : base(id, firstName, lastName, gender)
        {
            LicenseTypeTeaching = new List<LicenceType>();
            Schedule = new WeekSchedule((int)Configuration.NumbersOfWorkDaysInWeekTesters);
            Experience = 0;
            MaxWeekExams = 0;
            _maxDistance = 0;
        }




        public override string ToString()
        {
            return base.ToString() + " ,Job: A Tester ";
        }
    }
}
