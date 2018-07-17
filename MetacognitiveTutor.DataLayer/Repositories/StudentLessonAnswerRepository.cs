using System.Collections.Generic;
using MetacognitiveTutor.DataLayer.Models;

namespace MetacognitiveTutor.DataLayer.Repositories
{
    public class StudentLessonAnswerRepository : Repository<StudentLessonAnswer>
    {
        public List<StudentLessonAnswer> GetAllByProviderAndProviderIdAndLessonId(string provider, string providerId, int lessonId)
        {
            return Database.Fetch<StudentLessonAnswer>("WHERE (Provider = @0) AND (ProviderId = @1) AND (LessonId = @2)", provider, providerId, lessonId);
        }

        public List<StudentLessonAnswer> GetAllByLessonId(int lessonId)
        {
            return Database.Fetch<StudentLessonAnswer>("WHERE (LessonId = @0)", lessonId);
        }
    }
}
