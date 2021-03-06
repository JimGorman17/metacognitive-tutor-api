﻿using System;
using MetacognitiveTutor.DataLayer.Models;

namespace MetacognitiveTutor.DataLayer.Repositories
{
    public class GradeRepository : Repository<GradeModel>
    {
        public GradeModel GetGrade(int lessonId, string studentProvider, string studentProviderId)
        {
            return Database.SingleOrDefault<GradeModel>("WHERE (IsDeleted = @0) AND (LessonId = @1) AND (StudentProvider = @2) AND (StudentProviderId = @3)", false, lessonId, studentProvider, studentProviderId);
        }

        public void DeleteGrade(int gradeId)
        {
            Database.Update<GradeModel>("SET [UpdateDateUtc]=@0, IsDeleted=@1 WHERE (Id = @2)", DateTime.UtcNow, true, gradeId);
        }
    }
}
