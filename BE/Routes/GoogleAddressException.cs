using System;

namespace BE.Routes
{
    /// <inheritdoc />
    /// <summary>
    ///     Google address exception
    /// </summary>
    public class GoogleAddressException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        ///     A new Exception
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="errorCode">The code</param>
        public GoogleAddressException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        ///     Error code
        /// </summary>
        public string ErrorCode { get; }
    }
}