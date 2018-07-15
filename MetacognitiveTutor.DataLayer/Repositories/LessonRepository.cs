using System;
using MetacognitiveTutor.DataLayer.Models;

namespace MetacognitiveTutor.DataLayer.Repositories
{
    public class LessonRepository : Repository<Lesson>
    {
        public void DeleteLesson(int lessonId)
        {
            Database.Update<Lesson>("SET [UpdateDateUtc]=@0, Deleted=@1 WHERE (Id = @2)", DateTime.UtcNow, true, lessonId);
        }
    }
}
