using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class Guard
    {
        public static void NotNull(object value, string parameterName, string exceptionMessage = null)
        {
            if(value == null)
            {
                if (string.IsNullOrWhiteSpace(exceptionMessage))
                {
                    throw new ArgumentNullException(string.IsNullOrWhiteSpace(parameterName) ? nameof(value) : parameterName);
                }

                throw new ArgumentNullException(string.IsNullOrWhiteSpace(parameterName) ? nameof(value) : parameterName, exceptionMessage);
            }
        }

        public static void NotNullOrWhitespace(string value, string parameterName, string exceptionMessage = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrWhiteSpace(exceptionMessage))
                {
                    throw new ArgumentNullException(string.IsNullOrWhiteSpace(parameterName) ? nameof(value) : parameterName);
                }

                throw new ArgumentNullException(string.IsNullOrWhiteSpace(parameterName) ? nameof(value) : parameterName);
            }
        }
    }
}
