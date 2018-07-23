using System.Net;

namespace MetacognitiveTutor.Api.Services
{
    // ReSharper disable once UnusedMember.Global
    public class HttpStatusResult
    {
        private readonly HttpStatusCode _httpStatusCode;
        public HttpStatusResult(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        // ReSharper disable once UnusedMember.Global
        public int HttpStatusCode => (int)_httpStatusCode;
        // ReSharper disable once UnusedMember.Global
        public string Message => _httpStatusCode.ToString();
    }
}