using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class Factory_BL
    {
        static BlImp bl = null;

        public static BlImp GetObject
        {
            get
            {
                if (bl == null)
                    bl = new BlImp();
                return bl;
            }
        }

    }
}
