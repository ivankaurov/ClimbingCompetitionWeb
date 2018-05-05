namespace Climbing.Web.Utilities
{
    using System;

    public static class Guard
    {
        public static void NotNull(object value, string parameterName, string exceptionMessage = null)
        {
            if (value == null)
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

                throw new ArgumentNullException(string.IsNullOrWhiteSpace(parameterName) ? nameof(value) : parameterName, exceptionMessage);
            }
        }

        public static void Requires(bool predicate, string parameterName = null, string exceptionMessage = null)
        {
            if (!predicate)
            {
                if (string.IsNullOrWhiteSpace(parameterName))
                {
                    if (string.IsNullOrWhiteSpace(exceptionMessage))
                    {
                        throw new ArgumentException();
                    }

                    throw new ArgumentException(exceptionMessage);
                }

                if (string.IsNullOrWhiteSpace(exceptionMessage))
                {
                    throw new ArgumentException(parameterName);
                }

                throw new ArgumentException(exceptionMessage, parameterName);
            }
        }
    }
}
