using System;
using System.Linq;

namespace BE
{
    public class WeekSchedule : ICloneable
    {
        //default days
        private const int DefaultWeekDays = 5;

        /// <summary>
        ///     Days in week
        /// </summary>
        public Day[] Days;

        /// <summary>
        ///     a new schedule
        /// </summary>
        /// <param name="days">the days in the week between 1-7</param>
        public WeekSchedule(int days = DefaultWeekDays)
        {
            Days = days < 8 ? new Day[days] : new Day[DefaultWeekDays];
            for (var i = 0; i < Days.Length; i++) Days[i] = new Day((DayOfWeek) i);
        }

        /// <summary>
        ///     get a day
        /// </summary>
        /// <param name="d">the day</param>
        /// <returns>the day or null if it don't exist </returns>
        public Day this[DayOfWeek d]
        {
            private set { }
            get => (int) d < Days.Length ? Days[(int) d] : null;
        }

        /// <summary>
        ///     Clone the week
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var newDays = new Day[Days.Length];
            var i = 0;
            foreach (var item in Days)
            {
                if (item != null)
                    newDays[i] = item.Clone() as Day;
                i++;
            }

            return new WeekSchedule(Days.Length) {Days = newDays};
        }

        /// <summary>
        ///     check if is available on the day and hour
        /// </summary>
        /// <param name="day">the day</param>
        /// <param name="hour">the hour</param>
        /// <returns></returns>
        public bool IsAvailable(DayOfWeek day, int hour)
        {
            if ((int) day < Days.Length) return Days[(int) day].IsWorking(hour);
            throw new Exception("day out of range");
        }

        /// <summary>
        ///     the whole week schedule
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Days.Aggregate("", (current, d) => current + d.ToString() + ", ");
        }
    }
}