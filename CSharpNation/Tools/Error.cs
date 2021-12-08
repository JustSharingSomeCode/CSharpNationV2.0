using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Tools
{
    public class Error
    {
        public Error(Type errorType, String message)
        {
            ErrorType = errorType;
            Message = message;
        }

        public enum Type
        {
            Information,
            NonCriticalError,
            CriticalError
        }

        public Type ErrorType { get; private set; }
        public string Message { get; private set; }
    }
}
