using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Tools
    {
        /// <summary>
        /// Check if Israely ID is valied
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>true if it is valied</returns>
        public static bool CheckID_IL(uint id)
        {
            uint[] idArr = new uint[9];
            for (int i = 8; i >= 0; i--)
            {
                idArr[i] = id % 10;
                id /= 10;
            }
            for (uint i = 0; i < 9; i++)
            {
                idArr[i] *= i % 2 + 1;
            }
            for (uint i = 0; i < 9; i++)
            {
                idArr[i] = idArr[i] / 10 + idArr[i] % 10;
            }
            uint sum = 0;
            for (uint i = 0; i < 9; i++)
            {
                sum += idArr[i];
            }
            if (sum % 10 != 0)
                return false;
            return true;
        }

    }
}
