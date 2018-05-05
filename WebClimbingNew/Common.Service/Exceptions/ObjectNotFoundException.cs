namespace Climbing.Web.Common.Service.Exceptions
{
    using System;

    public class ObjectNotFoundException : Exception
    {
        private const string DefaultMessage = "Object not found";

        public ObjectNotFoundException()
            : base(DefaultMessage)
        {
        }

        public ObjectNotFoundException(string message)
            : base(message)
        {
        }

        public ObjectNotFoundException(Exception inner)
            : base(DefaultMessage, inner)
        {
        }

        public ObjectNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}