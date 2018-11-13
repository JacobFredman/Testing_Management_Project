﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   
   public class Tester:Person
    {
        public uint Experience { get; set ; }
        public uint MaxWeekExams { set; get; }
        public List<LicenceType> LicenceTypeTeaching { set; get; }
        private float maxDistance;
        public float MaxDistance { get => maxDistance; set { if (value >= 0) maxDistance = value; } }
        public WeekSchedule Scedual { set; get; }
        /// <summary>
        /// A new Tester
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="Fn">first name</param>
        /// <param name="Ln">last name</param>
        public Tester(uint id, string Fn = null, string Ln = null) : base(id,Fn,Ln) {
            LicenceTypeTeaching = new List<LicenceType>();
            Scedual = new WeekSchedule((int)Configuration.NumbersOfWorkDaysInWeekTesters);


        }
        public override string ToString()
        {
            return base.ToString() + " ,Job: A Tester ";
           
        }








    }
}
