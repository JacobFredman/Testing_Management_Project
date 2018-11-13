using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// static configurations for the program
    /// </summary>
   public  class Configuration
    {
        public static uint MinLessons;
        public static uint MinTesterAge;
        public static uint MinTraineeAge;
        public static uint MinTimeBetweenTests;
        public static uint NumbersOfWorkDaysInWeekTesters = 5; //for testers
        public static uint MinimumCritirionstoPassTest = 3;
    }
}
