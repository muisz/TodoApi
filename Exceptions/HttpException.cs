namespace TodoApi.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;

        public HttpException() { }
        public HttpException(string message) : base(message) { }
        public HttpException(string message, Exception inner) : base(message, inner) { }
        public HttpException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}