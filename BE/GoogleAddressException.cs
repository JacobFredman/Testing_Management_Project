using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GoogleAddressException :Exception
    {
        public string ErrorCode { set; get; }
        public GoogleAddressException(string message,string errorCode):base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
