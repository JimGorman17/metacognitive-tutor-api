using System;
using System.Collections.Generic;
using System.Linq;
using MetacognitiveTutor.DataLayer.Dtos;
using MetacognitiveTutor.DataLayer.Models;
using PetaPoco;

namespace MetacognitiveTutor.DataLayer.Repositories
{
    public class LessonRepository : Repository<Lesson>
    {
        public List<LessonDto> GetAllNonDeleted()
        {
            var query = Sql.Builder
                .Append("WITH cte_enrolled_students")
                .Append("AS")
                .Append("(")
                .Append("	SELECT LessonId, [Provider], ProviderId FROM [dbo].[StudentLessonAnswers] SLA GROUP BY LessonId, SLA.[Provider], SLA.ProviderId")
                .Append(")")
                .Select(
                      "[Id]"
                    , "BookTitle"
                    , "BookAmazonUrl"
                    , "TheHookYouTubeVideo"
                    , "TheTwoVocabularyWordsYouTubeVideo"
                    , "MainIdea"
                    , "SupportingIdea"
                    , "StoryDetails"
                    , "StoryQuestions"
                    , "ImportantSentencesForWordScramble"
                    , "Provider"
                    , "ProviderId"
                    , "UpdateDateUtc"
                    , "IsDeleted"
                    , "(SELECT COUNT(1) FROM cte_enrolled_students CES WHERE CES.LessonId = L.Id) AS [NumberOfEnrolledStudents]")
                .From("[dbo].[Lessons] L")
                .Where("IsDeleted = @0", false);
            
            return Database.Query<LessonDto>(query).ToList();
        }

        public void DeleteLesson(int lessonId)
        {
            Database.Update<Lesson>("SET [UpdateDateUtc]=@0, IsDeleted=@1 WHERE (Id = @2)", DateTime.UtcNow, true, lessonId);
        }
    }
}
