using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class GradeService : Service
    {
        public UserRepository UserRepository { get; set; }
        public GradeRepository GradeRepository { get; set; }
    }
}