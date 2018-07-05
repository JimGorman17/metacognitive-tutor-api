using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;

namespace MetacognitiveTutor.Api.Helpers
{
    public static class UserHelpers
    {
        public static User GetExistingUser(IProviderRequest request, UserRepository userRepository)
        {
            User existingUser = null;
            if (string.IsNullOrWhiteSpace(request.Provider) == false && string.IsNullOrWhiteSpace(request.ProviderId) == false)
            {
                try
                {
                    existingUser = userRepository.GetUserByProviderAndProviderId(request.Provider, request.ProviderId);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {
                }
            }

            return existingUser ?? new User();
        }
    }
}