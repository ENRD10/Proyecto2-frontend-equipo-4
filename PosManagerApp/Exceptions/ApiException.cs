using System;

namespace PosManagerApp.Exceptions
{
    public class ApiException : Exception
    {
        public int? StatusCode { get; }
        public string? ResponseContent { get; }

        public ApiException(string message, int? statusCode = null, string? responseContent = null, Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nStatusCode: {StatusCode}\nResponseContent: {ResponseContent}";
        }
    }
}
