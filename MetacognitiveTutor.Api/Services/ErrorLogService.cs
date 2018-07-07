using MetacognitiveTutor.Api.Helpers;
using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    // ReSharper disable once UnusedMember.Global
    public class ErrorLogService : Service
    {
        public Repository<ErrorLog> ErrorLogRepository { get; set; }
        public UserRepository UserRepository { get; set; }

        [Route("/error-log/create", "POST")]
        public class ErrorLogCreateRequest : IProviderRequest, IReturnVoid
        {
            [ApiMember(IsRequired = true)]
            public string Application { get; set; }
            [ApiMember(IsRequired = true)]
            public string Provider { get; set; }
            [ApiMember(IsRequired = true)]
            public string ProviderId { get; set; }
            public string ErrorMessage { get; set; }
            public string StackTrace { get; set; }
        }

        // ReSharper disable once UnusedMember.Global
        public void Post(ErrorLogCreateRequest request)
        {
            Guard.AgainstEmpty(request.Application);
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            Guard.IsTrue(r => string.IsNullOrWhiteSpace(r.ErrorMessage) == false || string.IsNullOrWhiteSpace(r.StackTrace), request);

            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);

            ErrorLogRepository.Add(new ErrorLog
            {
                Application = request.Application,
                Message = request.ErrorMessage,
                StackTrace = request.StackTrace,
                UserId = 0 < existingUser.Id ? existingUser.Id : (int?)null
            });
        }
    }
}