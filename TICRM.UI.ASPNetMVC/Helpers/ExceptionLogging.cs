using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TICRM.UI.ASPNetMVC.Helpers
{
    public class ExceptionLogging
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExceptionLogging));

        public static void LogException(Exception ex)
        {
            // Get the class name and method name where the exception occurred
            var className = ex.TargetSite?.DeclaringType?.FullName;
            var methodName = ex.TargetSite?.Name;

            // Get the stack trace
            var stackTrace = ex.StackTrace;

            // Get the exception message
            var exceptionMessage = ex.Message;

            // Log the exception details
            log.ErrorFormat("\nException Details:\nController Name/Class Name: {0}\nMethod Name: {1}\nStack Trace:\n{2}\nException Message: {3}\n",
                className, methodName, stackTrace, exceptionMessage);
        }
    }
}