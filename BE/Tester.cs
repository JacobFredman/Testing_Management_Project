using System.Collections.Generic;

namespace BE
{
   
   public class Tester:Person
    {
        private readonly Gender _gender;
        public uint Experience { get; set ; }
        public uint MaxWeekExams { set; get; }
        public List<LicenceType> LicenceTypeTeaching { set; get; }
        private float _maxDistance;
        public float MaxDistance { get => _maxDistance; set { if (value >= 0) _maxDistance = value; } }
        public WeekSchedule Scedule { set; get; }

        /// <summary>
        /// A new Tester
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        /// <param name="gender">tester gender</param>
        public Tester(string id, string firstName = "", string lastName = "",Gender gender=Gender.Male) : base(id,firstName,lastName,gender) {
            _gender = gender;
            LicenceTypeTeaching = new List<LicenceType>();
            Scedule = new WeekSchedule((int)Configuration.NumbersOfWorkDaysInWeekTesters);
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
