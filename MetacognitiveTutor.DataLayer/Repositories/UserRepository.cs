using MetacognitiveTutor.DataLayer.Models;

namespace MetacognitiveTutor.DataLayer.Repositories
{
    public class UserRepository : Repository<User>
    {
        public User GetUserByProviderAndProviderId(string provider, string providerId)
        {
            return Database.SingleOrDefault<User>("WHERE (Provider = @0) AND (ProviderId = @1)", provider, providerId);
        }
    }
}
