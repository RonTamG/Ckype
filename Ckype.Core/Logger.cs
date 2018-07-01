

using System.Diagnostics;

namespace Ckype.Core
{
    /// <summary>
    /// A class to enable message logging across the application
    /// </summary>
    public static class Logger
    {
        public static void LogMessage(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
