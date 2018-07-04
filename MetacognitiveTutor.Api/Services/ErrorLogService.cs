using System;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class ErrorLogService : Service
    {
        public Repository<ErrorLog> ErrorLogRepository { get; set; }
        public UserRepository UserRepository { get; set; }

        [Route("/ErrorLog/Create")]
        public class ErrorLogCreateRequest : IReturnVoid
        {
            public string Provider { get; set; }
            public string ProviderId { get; set; }
            public string ErrorMessage { get; set; }
            public string StackTrace { get; set; }
        }

        public void Post(ErrorLogCreateRequest request)
        {
            var existingUser = new User();

            if (string.IsNullOrWhiteSpace(request.Provider) == false && string.IsNullOrWhiteSpace(request.ProviderId))
            {
                try
                {
                    existingUser = UserRepository.GetUserByProviderAndProviderId(request.Provider, request.ProviderId);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {
                }
            }

            ErrorLogRepository.Add(new ErrorLog
            {
                Message = request.ErrorMessage,
                StackTrace = request.StackTrace,
                CreateDateUtc = DateTime.UtcNow,
                UserId = 0 < existingUser.Id ? existingUser.Id : (int?)null
            });
        }

    }
}