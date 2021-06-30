using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNationV2._0.Configuration
{
    public static class ErrorManager
    {
        public static List<string> ErrorMessages { get; private set; } = new List<string>();

        public static event EventHandler ErrorAdded;

        public static void AddErrorMessage(string message)
        {
            ErrorMessages.Add(message);
            ErrorAdded?.Invoke(message, EventArgs.Empty);
        }        
    }
}
