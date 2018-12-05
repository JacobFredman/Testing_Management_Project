using System;

namespace BE.Routes
{
    /// <summary>
    /// Google address exception
    /// </summary>
    public class GoogleAddressException :Exception
    {
        /// <summary>
        /// Error code
        /// </summary>
        public string ErrorCode { set; get; }

        /// <summary>
        /// A new Expection
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="errorCode">The code</param>
        public GoogleAddressException(string message,string errorCode):base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
