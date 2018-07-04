using MetacognitiveTutor.DataLayer.Interfaces;
using PetaPoco;

namespace MetacognitiveTutor.DataLayer.Models
{
    [TableName("Users")]
    [PrimaryKey("Id")]
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderPic { get; set; }

        [Ignore]
        public bool IsNew => Id == default(int);
    }
}
