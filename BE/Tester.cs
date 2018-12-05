using System.Collections.Generic;

namespace BE
{
   
   public class Tester:Person
    {
        /// <summary>
        /// experience of the tester in years
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
        /// Max travel distance
        /// </summary>
        public float MaxDistance { get => _maxDistance; set { if (value >= 0) _maxDistance = value; } }

        /// <summary>
        /// Week schedule
        /// </summary>
        public WeekSchedule Schedule { set; get; }

        /// <summary>
        /// A new Tester
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        /// <param name="gender">tester gender</param>
        public Tester(string id, string firstName = "", string lastName = "",Gender gender=Gender.Male) : base(id,firstName,lastName,gender) {
            LicenseTypeTeaching = new List<LicenseType>();
            Schedule = new WeekSchedule((int)Configuration.NumbersOfWorkDaysInWeekTesters);
            Experience = 0;
            MaxWeekExams = 0;
            _maxDistance = 0;
        }

        //Information about the tester
        public override string ToString()
        {
            return base.ToString() + " ,Job: A Tester ";
        }
    }
}
