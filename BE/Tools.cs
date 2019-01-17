using System;
using System.Globalization;

namespace BE
{
    public static class Tools
    {
        /// <summary>
        ///     Check if Israel Id is valid
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>true if id number  valid</returns>
        public static bool CheckID_IL(uint id)
        {
            if (id == 0) return false;
            var idArr = new uint[9];

            //put the Id in an arr
            for (var i = 8; i >= 0; i--)
            {
                idArr[i] = id % 10;
                id /= 10;
            }

            //multiply the odd digits and add one
            for (uint i = 0; i < 9; i++) idArr[i] *= i % 2 + 1;

            //sum the digits of the numbers
            for (uint i = 0; i < 9; i++) idArr[i] = idArr[i] / 10 + idArr[i] % 10;

            //Sum all the numbers
            uint sum = 0;
            for (uint i = 0; i < 9; i++) sum += idArr[i];


            return sum % 10 == 0;
        }

        /// <summary>
        ///     Get an age from a birth date
        /// </summary>
        /// <param name="birthDate">The birth date</param>
        /// <returns>The age in years</returns>
        public static int GetAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }


        /// <summary>
        ///     Check if two dates are in the same week
        /// </summary>
        /// <param name="date1">first date</param>
        /// <param name="date2">second date</param>
        /// <returns></returns>
        public static bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));
            return d1 == d2;
        }

    }
}