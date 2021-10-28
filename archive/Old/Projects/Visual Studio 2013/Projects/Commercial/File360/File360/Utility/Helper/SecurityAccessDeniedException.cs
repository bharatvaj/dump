using System;

namespace File360.Utility.Helpers
{
    internal class SecurityAccessDeniedException : Exception
    {
        public SecurityAccessDeniedException()
        {
        }

        public SecurityAccessDeniedException(string message) : base(message)
        {
        }

        public SecurityAccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}