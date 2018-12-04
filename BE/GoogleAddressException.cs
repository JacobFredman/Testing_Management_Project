using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
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
