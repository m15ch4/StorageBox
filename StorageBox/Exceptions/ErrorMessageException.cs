using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Exceptions
{
    public class ErrorMessageException : Exception
    {
        public ErrorMessageException()
        {
        }

        public ErrorMessageException(string message) : base(message)
        {
        }

        public ErrorMessageException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
