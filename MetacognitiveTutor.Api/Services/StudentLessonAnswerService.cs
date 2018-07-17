using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class StudentLessonAnswerService : Service
    {
        public UserRepository UserRepository { get; set; }
        public LessonRepository LessonRepository { get; set; }
        public StudentLessonAnswerRepository StudentLessonAnswerRepository { get; set; }
    }
}