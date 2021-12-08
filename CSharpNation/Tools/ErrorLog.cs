using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Tools
{
    static class ErrorLog
    {
        static ErrorLog()
        {
            errorMessages = new List<Error>();
        }

        private static List<Error> errorMessages;

        public static void AddError(Error error)
        {
            errorMessages.Add(error);
            try
            {
                OnErrorAdded?.Invoke(error, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static EventHandler OnErrorAdded;
    }
}
