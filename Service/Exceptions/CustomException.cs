﻿using System.Net;

namespace Service.Exceptions
{
    public abstract class CustomException : Exception
    {
        private readonly HttpStatusCode StatusCode;

        protected CustomException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public CustomException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

    }
}
