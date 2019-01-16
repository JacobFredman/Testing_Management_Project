using System;
using System.Collections;

namespace BE
{
    /// <inheritdoc />
    /// <summary>
    ///     A day schedule
    /// </summary>
    public class Day :  ICloneable
    {
        /// <summary>
        ///     the hours in the day. if available then is true
        /// </summary>
        public bool[] Hours = new bool[24];

        /// <summary>
        ///     the day in the week
        /// </summary>
        public DayOfWeek TheDay { set; get; }

        /// <summary>
        ///     A new day
        /// </summary>
        /// <param name="d">the day</param>
        public Day(DayOfWeek d = DayOfWeek.Sunday)
        {
            ClearHours();
            TheDay = d;
        }
    
        /// <summary>
        /// Clone the day
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Day {TheDay = TheDay, Hours = Hours.Clone() as bool[]};
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
        /// minimum hour that tester works
        /// </summary>
        /// <returns></returns>
        public int MaxHourWorking()
        {
            for (int i = 23; i > 0; i--)
            {
                if (Hours[i] == true)
                    return i;
            }

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