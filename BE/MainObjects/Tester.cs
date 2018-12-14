using System;
using System.Collections.Generic;
using BE.Routes;

namespace BE.MainObjects
{
   /// <summary>
   /// A tester
   /// </summary>
   public class Tester:Person
    {
        /// <summary>
        /// Tester expirience
        /// </summary>
        public uint Experience { get; set ; }

        /// <summary>
        /// Max exams in week
        /// </summary>
        public uint MaxWeekExams { set; get; }

        /// <summary>
        /// License type teaching
        /// </summary>
        public List<LicenseType> LicenseTypeTeaching { set; get; }

        private float _maxDistance;

        /// <summary>
        /// Max distance
        /// </summary>
        public float MaxDistance
        {
            get => _maxDistance;
            set { if (value >= 0) _maxDistance = value; }
        }

        /// <summary>
        /// week schedule
        /// </summary>
        public WeekSchedule Schedule { set; get; }

        /// <summary>
        /// An new tester
        /// </summary>
        public Tester() { }

        /// <inheritdoc />
        /// <summary>
        /// A new Tester
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        /// <param name="gender">tester gender</param>
        public Tester(uint Id, string firstName = "", string lastName = "", Gender gender = Gender.Male) : base(Id, firstName, lastName, gender)
        {
            LicenseTypeTeaching = new List<LicenseType>();
            Schedule = new WeekSchedule((int)Configuration.NumbersOfWorkDaysInWeekTesters);
            Experience = 0;
            MaxWeekExams = 0;
            _maxDistance = 0;
        }

        /// <summary>
        /// An new tester
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
        public Tester(uint Id, string firstName, string lastName, Gender gender, string emailAddress, DateTime birthDate, string phoneNumber, Address address, List<LicenseType> licenseTypes,
                   uint experience, uint maxWeekExams, List<LicenseType> licenseTypeTeaching, float maxDistance) :
               base(Id, firstName, lastName, gender, emailAddress, birthDate, phoneNumber, address, licenseTypes)
        {
            Experience = experience;
            MaxWeekExams = maxWeekExams;
            LicenseTypeTeaching = licenseTypeTeaching;
            MaxDistance = maxDistance;
        }


        /// <summary>
        /// Basic data about tester
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + " ,Job: A Tester ";
        }
    }
}
