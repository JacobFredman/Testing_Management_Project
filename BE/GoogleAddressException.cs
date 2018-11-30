using System;

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
