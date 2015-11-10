using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aegis
{
    public class SingletonException : Exception
    {
        public SingletonException() : base() { }

        public SingletonException(string message) : base(message) { }

        public SingletonException(string message, Exception innerException) : base(message, innerException) { }
    }
}
