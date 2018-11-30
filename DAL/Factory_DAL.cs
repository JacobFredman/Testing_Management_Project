using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public static  class FactoryDal
    {
        private static DalImp _dal;
        public static DalImp GetObject {
            set { }
            get
            {
                if(_dal==null)
                    _dal=new DalImp();
                return _dal;
            }
        }
    }
}
