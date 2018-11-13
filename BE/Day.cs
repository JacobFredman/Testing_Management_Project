using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Day:IEnumerable
    {
        private  bool[]hours=new bool[24];
        public Days TheDay=Days.Sunday;
        /// <summary>
        /// A new day
        /// </summary>
        /// <param name="d">the day</param>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public Day(Days d,params uint[] range)
        {
            ClearHours();
            if (range != null)
                SetHours(range);
            TheDay = d;
        }
        /// <summary>
        /// add hours to day
        /// </summary>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void AddHours( params uint[] range)
        {
            if (range == null)
                return;
            if (range.Length % 2 != 0)
                throw new Exception("Invalied hour format");
           int i = 0;
           for( i = 0; i < range.Length-1; i+=2)
            {
                if (range[i] > range[i + 1])
                    throw new Exception("Invalied hours range");
                for(uint j = range[i]; j <= range[i + 1]; j++)
                {
                    hours[j] = true;
                }
            }
            if (i != range.Length)
                hours[range[i]] = true;
        }
        /// <summary>
        /// remove hours
        /// </summary>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void RemoveHours(params uint[] range)
        {
            if (range == null)
                return;
            if (range.Length % 2 != 0)
                throw new Exception("Invalied hour format");
            int i = 0;
            for (i = 0; i < range.Length - 1; i += 2)
            {
                if (range[i] > range[i + 1])
                    throw new Exception("Invalied hours range");
                for (uint j = range[i]; j <= range[i + 1]; j++)
                {
                    hours[j] = false;
                }
            }
            if (i != range.Length)
                hours[range[i]] = true;
        }
        /// <summary>
        /// clear hours
        /// </summary>
        public void ClearHours()
        {
            for (int i = 0; i < 24; i++)
                hours[i] = false;
        }
        /// <summary>
        /// set new hours for the whole day
        /// </summary>
        /// <param name="range">the hours. for exsample (12 ,13) will be 12:00-13:00. (12,12) will add only 12:00 .add only in pairs!</param>
        public void SetHours(params uint[] range)
        {
            ClearHours();
            AddHours(range);
        }
        /// <summary>
        /// check if he is working on the hour
        /// </summary>
        /// <param name="i">the hour. for example 12</param>
        /// <returns>true if he is working</returns>
        public bool IsWorking(int i)
        {
            if (i < 0 || i > 24)
                throw new Exception("Hour is not valied");
            return hours[i];
        }
        /// <summary>
        /// latest hour that he is working
        /// </summary>
        /// <returns>the hour</returns>
        public int LatestWorkHour()
        {
            for(int i = 23; i >= 0; i--)
            {
                if (hours[i])
                    return i;
               
            }
            return -1;
        }
        /// <summary>
        /// the urliest hour that he is working
        /// </summary>
        /// <returns>the hours</returns>
        public int UrliestWorkHour()
        {
            for (int i = 0; i <24; i++)
            {
                if (hours[i])
                    return i;

            }
            return -1;
        }
        public override string ToString()
        {
            string str=TheDay.ToString()+": ";
            int j;
            for (int i = 0; i < 24; i++)
            {
                if (hours[i])
                {
                    for ( j = i ; j < 23 && hours[j+1]; j++) ;
                    if (j != i)
                        str += string.Format("{0:00}:00 - {1:00}:00 ,", i, j);
                    else
                        str += string.Format("{0:00}:00 ,", i);
                    i = j;
                }
            }
            str = str.TrimEnd(',');
            return str;
        }

        public IEnumerator GetEnumerator()
        {
            return hours.GetEnumerator();
        }
    }
}
