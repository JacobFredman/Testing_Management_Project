﻿using System;

namespace BE
{
    /// <summary>
    ///     Licnese lessons
    /// </summary>
    public class LessonsAndType : ICloneable
    {
        private int _numberOfLessons;

        /// <summary>
        ///     license
        /// </summary>
        public LicenseType License { set; get; }

        /// <summary>
        ///     number of lessons
        /// </summary>
        public int NumberOfLessons
        {
            get => _numberOfLessons;
            set
            {
                _numberOfLessons = value;
                ReadyForTest = NumberOfLessons > Configuration.MinLessons;
            }
        }

        //ready for test
        public bool ReadyForTest { set; get; }

        public object Clone()
        {
            return new LessonsAndType
                {License = License, NumberOfLessons = NumberOfLessons, ReadyForTest = ReadyForTest};
        }

        /// <summary>
        ///     lesson details
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "License type: " + License + ", Num of lessons: " + NumberOfLessons + ", ready for test: " +
                   (ReadyForTest ? "yes" : "no");
        }
    }
}