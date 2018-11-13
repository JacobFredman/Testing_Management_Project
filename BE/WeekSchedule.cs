using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public  class WeekSchedule:IEnumerable
    {
        private const int DEFUALT_WEEK_DAYS = 5;
        private Day[] days;
        /// <summary>
        /// a new scedual
        /// </summary>
        /// <param name="days">the days in the week betwwen 1-7</param>
        public WeekSchedule(int days = DEFUALT_WEEK_DAYS)
        {
            if(days<8)
               this.days = new Day[days];
            else
                this.days = new Day[DEFUALT_WEEK_DAYS];
            for (int i = 0; i < this.days.Length; i++)
            {
                this.days[i] = new Day((Days)i);
            }
        }
        /// <summary>
        /// add hours
        /// </summary>
        /// <param name="d">the day to add</param>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void AddHourToDay(Days d,params uint[] range)
        {
            days[(int)d].AddHours(range);
        }
        /// <summary>
        /// remove hours
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void RemoveHourFromDay(Days d, params uint[] range)
        {
            days[(int)d].RemoveHours(range);
        }
        /// <summary>
        /// replace hours in day
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void SetHourInDay(Days d, params uint[] range)
        {
            days[(int)d].SetHours(range);
        }
        /// <summary>
        /// clear the hours in a day
        /// </summary>
        /// <param name="d">the day</param>
        public void ClearHourInDay(Days d)
        {
            days[(int)d].ClearHours();
        }
        /// <summary>
        /// clear the whole week
        /// </summary>
        public void Clear()
        {
            foreach(Day d in days)
            {
                d.ClearHours();
            }
        }
        /// <summary>
        /// set the same hours for all the days
        /// </summary>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void SetHoursAllDays(params uint[] range)
        {
            foreach (Day d in days)
            {
                d.SetHours(range);
            }
        }
        /// <summary>
        /// add hours to all the days
        /// </summary>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void AddHoursAllDays(params uint[] range)
        {
            foreach (Day d in days)
            {
                d.AddHours(range);
            }
        }
        /// <summary>
        /// remove the hours for all the days
        /// </summary>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void RemoveHoursAllDays(params uint[] range)
        {
            foreach (Day d in days)
            {
                d.RemoveHours(range);
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
                if (i >= 0 && i < days.Length)
                    return days[i];
                else
                    return null;
                }
        }
        /// <summary>
        /// get a day
        /// </summary>
        /// <param name="d">the day</param>
        /// <returns>the day or null if it don't exist </returns>
        public Day this[Days d]
        {
            private set { }
            get
            {
                if ((int)d < days.Length)
                    return days[(int)d];
                else
                    return null;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return days.GetEnumerator();
        }
        public override string ToString()
        {
            string str = "";
            foreach(Day d in days)
            {
                str += d.ToString() + "\n";
            }
            return str;
        }
    }
}
