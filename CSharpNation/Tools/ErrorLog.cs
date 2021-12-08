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
            errorMessages = new List<string>();
        }

        private static List<string> errorMessages;

        public static void AddError(string message)
        {
            errorMessages.Add(message);
            try
            {
                OnErrorAdded?.Invoke(message, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static EventHandler OnErrorAdded;
    }
}
