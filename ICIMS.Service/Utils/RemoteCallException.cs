using System;
using System.Runtime.Serialization;
 

namespace ICIMS.Service
{
    /// <summary>
    /// This exception is thrown when a remote method call made and remote application sent an error message.
    /// </summary>
    [Serializable]
    public class RemoteCallException : ICIMSException
    {
        /// <summary>
        /// Remote error information.
        /// </summary>
        public ErrorInfo ErrorInfo { get; set; }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public RemoteCallException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public RemoteCallException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        /// <param name="errorInfo">Exception message</param>
        public RemoteCallException(ErrorInfo errorInfo)
            : base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
        }
    }
}
