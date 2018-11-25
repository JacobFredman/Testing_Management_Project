using System.Collections.Generic;

namespace BE
{
   
   public class Tester:Person
    {
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
        /// <param name="Fn">first name</param>
        /// <param name="Ln">last name</param>
        public Tester(uint id, string Fn = "", string Ln = "",Gender g=Gender.Male) : base(id,Fn,Ln,g) {
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
