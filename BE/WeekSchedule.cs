using System;
using System.Collections;

namespace BE
{
   public  class WeekSchedule:IEnumerable
    {
        private const int DefualtWeekDays = 5;
        private Day[] _days;
        /// <summary>
        /// a new schedual
        /// </summary>
        /// <param name="days">the days in the week between 1-7</param>
        public WeekSchedule(int days = DefualtWeekDays)
        {
            this._days = days<8 ? new Day[days] : new Day[DefualtWeekDays];
            for (int i = 0; i < this._days.Length; i++)
            {
                this._days[i] = new Day((DayOfWeek)i);
            }
        }
        /// <summary>
        /// add hours
        /// </summary>
        /// <param name="d">the day to add</param>
        /// <param name="range">the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void AddHourToDay(DayOfWeek d,params uint[] range)
        {
            _days[(int)d].AddHours(range);
        }
        /// <summary>
        /// remove hours
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void RemoveHourFromDay(DayOfWeek d, params uint[] range)
        {
            _days[(int)d].RemoveHours(range);
        }
        /// <summary>
        /// replace hours in day
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void SetHourInDay(DayOfWeek d, params uint[] range)
        {
            _days[(int)d].SetHours(range);
        }
        /// <summary>
        /// clear the hours in a day
        /// </summary>
        /// <param name="d">the day</param>
        public void ClearHourInDay(DayOfWeek d)
        {
            _days[(int)d].ClearHours();
        }
        /// <summary>
        /// clear the whole week
        /// </summary>
        public void Clear()
        {
            foreach(Day d in _days)
            {
                d.ClearHours();
            }
        }
        /// <summary>
        /// set the same hours for all the days
        /// </summary>
        /// <param name="range">the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void SetHoursAllDays(params uint[] range)
        {
            foreach (Day d in _days)
            {
                d.SetHours(range);
            }
        }
        /// <summary>
        /// add hours to all the days
        /// </summary>
        /// <param name="range">the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void AddHoursAllDays(params uint[] range)
        {
            foreach (var day in _days)
            {
                day.AddHours(range);
            }
        }
        /// <summary>
        /// remove the hours for all the days
        /// </summary>
        /// <param name="range">the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void RemoveHoursAllDays(params uint[] range)
        {
            foreach (var day in _days)
            {
                day.RemoveHours(range);
            }
        }
        /// <summary>
        /// get a day
        /// </summary>
        /// <param name="i">the day. between 0-7</param>
        /// <returns>the day or null if it don't exist</returns>
        public Day this[int i] {
            private set { }
            get {
                if (i >= 0 && i < _days.Length)
                    return _days[i];
                else
                    return null;
                }
        }
        /// <summary>
        /// get a day
        /// </summary>
        /// <param name="d">the day</param>
        /// <returns>the day or null if it don't exist </returns>
        public Day this[DayOfWeek d]
        {
            private set { }
            get
            {
                if ((int)d < _days.Length)
                    return _days[(int)d];
                else
                    return null;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _days.GetEnumerator();
        }

        /// <summary>
        /// check if is availble on the day and hour
        /// </summary>
        /// <param name="day">the day</param>
        /// <param name="hour">the hour</param>
        /// <returns></returns>
        public bool IsAvailable(DayOfWeek day, int hour)
        {
            if ((int)day < _days.Length)
            {
                return _days[(int)day].IsWorking(hour);
            }
            throw new Exception("day out of range");
        }
        public override string ToString()
        {
            string str = "";
            foreach(Day d in _days)
            {
                str += d.ToString() + "\n";
            }
            return str;
        }
    }
}
