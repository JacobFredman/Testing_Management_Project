using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Licnese lessons
    /// </summary>
    public class LessonsAndType:ICloneable
    {
        private int _numberOfLessons;

        /// <summary>
        /// license
        /// </summary>
        public LicenseType License { set; get; }

        /// <summary>
        /// number of lessons
        /// </summary>
        public int NumberOfLessons { get => _numberOfLessons; set { _numberOfLessons = value; ReadyForTest = NumberOfLessons > Configuration.MinLessons; } }

        //ready for test
        public bool ReadyForTest { set; get; }

        public object Clone()
        {
            return new LessonsAndType() { License = this.License, NumberOfLessons = this.NumberOfLessons, ReadyForTest = this.ReadyForTest };
        }

        /// <summary>
        /// lesson details
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "License type: " + License.ToString() + ", Num of lessons: " + NumberOfLessons + ", ready for test: " + ((ReadyForTest) ? "yes" : "no");
        }
    }
}
