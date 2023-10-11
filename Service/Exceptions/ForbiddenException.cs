using System.Net;

namespace Service.Exceptions
{
    public class ForbiddenException : CustomException
    {
        public ForbiddenException() : base(HttpStatusCode.Forbidden)
        {
        }

        public ForbiddenException(string message)
            : base(message, HttpStatusCode.Forbidden)
        {
        }
    }
}
