using System;
using System.Collections.Generic;
using MetacognitiveTutor.DataLayer.Models;

namespace MetacognitiveTutor.DataLayer.Repositories
{
    public class LessonRepository : Repository<Lesson>
    {
        public List<Lesson> GetAllNonDeleted()
        {
            return Database.Fetch<Lesson>("WHERE (IsDeleted = @0)", false);
        }

        public void DeleteLesson(int lessonId)
        {
            Database.Update<Lesson>("SET [UpdateDateUtc]=@0, IsDeleted=@1 WHERE (Id = @2)", DateTime.UtcNow, true, lessonId);
        }
    }
}
