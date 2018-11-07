using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service
{

    [Serializable]
    public class ICIMSException : Exception
    {

        public ICIMSException()
        {

        }

        public ICIMSException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="ICIMSException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public ICIMSException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="ICIMSException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ICIMSException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

}
