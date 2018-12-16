using System;
using System.Collections;
using System.Linq;

namespace BE
{
    public class WeekSchedule : IEnumerable
    {
        private const int DEFUALT_WEEK_DAYS = 5;

        private readonly Day[] days;

        /// <summary>
        ///     a new schedule
        /// </summary>
        /// <param name="days">the days in the week between 1-7</param>
        public WeekSchedule(int days = DEFUALT_WEEK_DAYS)
        {
            this.days = days < 8 ? new Day[days] : new Day[DEFUALT_WEEK_DAYS];
            for (var i = 0; i < this.days.Length; i++) this.days[i] = new Day((DayOfWeek) i);
        }

        /// <summary>
        ///     get a day
        /// </summary>
        /// <param name="i">the day. between 0-7</param>
        /// <returns>the day or null if it don't exist</returns>
        public Day this[int i]
        {
            get
            {
                if (i >= 0 && i < days.Length)
                    return days[i];
                return null;
            }
        }

        /// <summary>
        ///     get a day
        /// </summary>
        /// <param name="d">the day</param>
        /// <returns>the day or null if it don't exist </returns>
        public Day this[DayOfWeek d]
        {
            private set { }
            get
            {
                if ((int) d < days.Length)
                    return days[(int) d];
                return null;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return days.GetEnumerator();
        }

        /// <summary>
        ///     add hours
        /// </summary>
        /// <param name="d">the day to add</param>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void AddHourToDay(DayOfWeek d, params uint[] range)
        {
            days[(int) d].AddHours(range);
        }

        /// <summary>
        ///     remove hours
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void RemoveHourFromDay(DayOfWeek d, params uint[] range)
        {
            days[(int) d].RemoveHours(range);
        }

        /// <summary>
        ///     replace hours in day
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void SetHourInDay(DayOfWeek d, params uint[] range)
        {
            days[(int) d].SetHours(range);
        }

        /// <summary>
        ///     clear the hours in a day
        /// </summary>
        /// <param name="d">the day</param>
        public void ClearHourInDay(DayOfWeek d)
        {
            days[(int) d].ClearHours();
        }

        /// <summary>
        ///     clear the whole week
        /// </summary>
        public void Clear()
        {
            foreach (var d in days) d.ClearHours();
        }

        /// <summary>
        ///     set the same hours for all the days
        /// </summary>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void SetHoursAllDays(params uint[] range)
        {
            foreach (var d in days) d.SetHours(range);
        }

        /// <summary>
        ///     add hours to all the days
        /// </summary>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void AddHoursAllDays(params uint[] range)
        {
            foreach (var day in days) day.AddHours(range);
        }

        /// <summary>
        ///     remove the hours for all the days
        /// </summary>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void RemoveHoursAllDays(params uint[] range)
        {
            foreach (var day in days) day.RemoveHours(range);
        }

        /// <summary>
        ///     check if is available on the day and hour
        /// </summary>
        /// <param name="day">the day</param>
        /// <param name="hour">the hour</param>
        /// <returns></returns>
        public bool IsAvailable(DayOfWeek day, int hour)
        {
            if ((int) day < days.Length) return days[(int) day].IsWorking(hour);
            throw new Exception("day out of range");
        }

        /// <summary>
        ///     the whole week schedule
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return days.Aggregate("", (current, d) => current + d.ToString() + "\n");
        }
    }
}