using System;
using MetacognitiveTutor.Api.Helpers;
using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class UserService : Service
    {
        public UserRepository UserRepository { get; set; }

        [Route("/user/login", "POST")]
        public class UserLoginRequest : IProviderRequest, IReturnVoid
        {
            public string Name { get; set; }
            public string Token { get; set; }
            public string Email { get; set; }
            [ApiMember(IsRequired = true)]
            public string Provider { get; set; }
            [ApiMember(IsRequired = true)]
            public string ProviderId { get; set; }
            public string ProviderPic { get; set; }
            public bool IsTeacher { get; set; }
            public bool IsStudent { get; set; }
        }

        // ReSharper disable once UnusedMember.Global
        public void Post(UserLoginRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            Guard.IsTrue(r => r.IsTeacher || r.IsStudent, request);

            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            if (existingUser.IsNew)
            {
                UserRepository.Add(new User
                {
                    Email = request.Email,
                    IsTeacher = request.IsTeacher,
                    IsStudent = request.IsStudent,
                    Name = request.Name,
                    Provider = request.Provider,
                    ProviderId = request.ProviderId,
                    ProviderPic = request.ProviderPic,
                    Token = request.Token
                });
            }
            else
            {
                if (existingUser.Email.Equals(request.Email, StringComparison.Ordinal) == false ||
                    existingUser.IsTeacher == false && request.IsTeacher ||
                    existingUser.IsStudent == false && request.IsStudent ||
                    existingUser.Name.Equals(request.Name, StringComparison.Ordinal) == false ||
                    existingUser.ProviderPic.GetUntilOrEmpty("&ext=").Equals(request.ProviderPic.GetUntilOrEmpty("&ext="), StringComparison.Ordinal) == false)
                {
                    existingUser.Email = request.Email;
                    existingUser.IsTeacher = existingUser.IsTeacher || request.IsTeacher;
                    existingUser.IsStudent = existingUser.IsStudent || request.IsStudent;
                    existingUser.Name = request.Name;
                    existingUser.ProviderPic = request.ProviderPic;
                    existingUser.UpdateDateUtc = DateTime.UtcNow;
                    UserRepository.Update(existingUser);
                }
                else if (existingUser.Token.Equals(request.Token, StringComparison.Ordinal) == false)
                {
                    existingUser.Token = request.Token;
                    existingUser.UpdateDateUtc = DateTime.UtcNow;
                    UserRepository.Update(existingUser);
                }
            }
        }
    }
}