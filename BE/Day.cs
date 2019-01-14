using System;
using System.Collections;

namespace BE
{
    // todo : day and hours too complexity
    /// <inheritdoc />
    /// <summary>
    ///     A day schedule
    /// </summary>
    public class Day : IEnumerable, ICloneable
    {
        /// <summary>
        ///     the hours in the day. if available then is true
        /// </summary>
        public bool[] Hours = new bool[24];

        /// <summary>
        ///     A new day
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public Day(DayOfWeek d = DayOfWeek.Sunday, params uint[] range)
        {
            ClearHours();
            if (range != null)
                SetHours(range);
            else
                ClearHours();
            TheDay = d;
        }

        /// <summary>
        ///     the day in the week
        /// </summary>
        public DayOfWeek TheDay { set; get; }

        public object Clone()
        {
            return new Day {TheDay = TheDay, Hours = Hours.Clone() as bool[]};
        }

        public IEnumerator GetEnumerator()
        {
            return Hours.GetEnumerator();
        }

        /// <summary>
        ///     add hours to day
        /// </summary>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void AddHours(params uint[] range)
        {
            if (range == null)
                return;
            if (range.Length % 2 != 0)
                throw new Exception("Invalid hour format");
            var i = 0;
            //add hours
            for (i = 0; i < range.Length - 1; i += 2)
            {
                if (range[i] > range[i + 1])
                    throw new Exception("Invalid hours range");
                for (var j = range[i]; j <= range[i + 1]; j++) Hours[j] = true;
            }

            if (i != range.Length)
                Hours[range[i]] = true;
        }

        /// <summary>
        ///     remove hours
        /// </summary>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void RemoveHours(params uint[] range)
        {
            if (range == null)
                return;
            if (range.Length % 2 != 0)
                throw new Exception("Invalid hour format");
            var i = 0;
            //remove the hours
            for (i = 0; i < range.Length - 1; i += 2)
            {
                if (range[i] > range[i + 1])
                    throw new Exception("Invalid hours range");
                for (var j = range[i]; j <= range[i + 1]; j++) Hours[j] = false;
            }

            if (i != range.Length)
                Hours[range[i]] = true;
        }

        /// <summary>
        ///     clear hours
        /// </summary>
        public void ClearHours()
        {
            for (var i = 0; i < 24; i++)
                Hours[i] = false;
        }

        /// <summary>
        ///     set new hours for the whole day
        /// </summary>
        /// <param name="range">
        ///     the hours. for example (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in
        ///     pairs!
        /// </param>
        public void SetHours(params uint[] range)
        {
            ClearHours();
            AddHours(range);
        }

        /// <summary>
        ///     check if he is working on the hour
        /// </summary>
        /// <param name="i">the hour. for example 12</param>
        /// <returns>true if he is working</returns>
        public bool IsWorking(int i)
        {
            if (i < 0 || i > 24)
                throw new Exception("Hour is not valid");
            return Hours[i];
        }

        /// <summary>
        ///     latest hour that he is working
        /// </summary>
        /// <returns>the hour</returns>
        public int LatestWorkHour()
        {
            for (var i = 23; i >= 0; i--)
                if (Hours[i])
                    return i;
            return -1;
        }

        /// <summary>
        ///     the earliest hour that he is working
        /// </summary>
        /// <returns>the hours</returns>
        public int EarliestWorkHour()
        {
            for (var i = 0; i < 24; i++)
                if (Hours[i])
                    return i;
            return -1;
        }

        /// <summary>
        ///     Get all the work hours in the day
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = TheDay + ": ";
            int j;
            for (var i = 0; i < 24; i++)
                if (Hours[i])
                {
                    for (j = i; j < 23 && Hours[j + 1]; j++) ;
                    if (j != i)
                        str += $"{i:00}:00 - {j:00}:00 ,";
                    else
                        str += $"{i:00}:00 ,";
                    i = j;
                }

            str = str.TrimEnd(',');
            return str;
        }
    }
}