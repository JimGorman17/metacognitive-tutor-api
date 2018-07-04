using System.Net;

namespace MetacognitiveTutor.Api.Services
{
    public class HttpStatusResult
    {
        private readonly HttpStatusCode _httpStatusCode;
        public HttpStatusResult(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        public int HttpStatusCode => (int)_httpStatusCode;
        public string Message => _httpStatusCode.ToString();
    }
}