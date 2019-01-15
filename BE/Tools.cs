using System;
using System.Net;
using System.Text;
using BE.Routes;
using Newtonsoft.Json.Linq;

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

    }
}