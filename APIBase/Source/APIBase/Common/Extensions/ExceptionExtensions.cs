using System;

namespace APIBase.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToStringException(this Exception ex)
        {
            var message = $"Message = {ex.Message}.\n" +
                          $"StackTrace: {ex.StackTrace}";

            if (ex.InnerException != null)
            {
                message = message + "\n\tInnerException: " + ex.InnerException.ToStringException(2);
            }

            return message;
        }

        private static string ToStringException(this Exception ex, int tabs)
        {
            var tabsString = "";
            for (int index = 1; index < tabs; index++)
            {
                tabsString += "\t";
            }

            var message = $"Message = {ex.Message}.\n" + tabsString +
                          $"StackTrace: {ex.StackTrace}";

            tabsString += "\t";

            if (ex.InnerException != null)
            {
                message = message + "\n" + tabsString + "InnerException: " + ex.InnerException.ToStringException(tabs + 1);
            }

            return message;
        }
    }
}
