using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    /// <summary>
    /// Get Instance of DAL
    /// </summary>
    public static  class FactoryDal
    {
        private static DalImp _dal=null;
        /// <summary>
        /// Get the object
        /// </summary>
        public static DalImp GetObject => _dal ?? (_dal = new DalImp());
    }
}
