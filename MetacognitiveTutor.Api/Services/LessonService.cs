using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MetacognitiveTutor.Api.Dtos;
using MetacognitiveTutor.Api.Helpers;
using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using MoreLinq.Extensions;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    // ReSharper disable once UnusedMember.Global
    public class LessonService : Service
    {
        public UserRepository UserRepository { get; set; }
        public LessonRepository LessonRepository { get; set; }
        public GradeRepository GradeRepository { get; set; }

        [Route("/lesson/getall", "POST")]
        public class LessonGetAllRequest : IProviderRequest, IReturn<IEnumerable<LessonResponse>>
        {
            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
        }

        [Route("/lesson/upsert", "POST")]
        public class LessonUpsertRequest : IProviderRequest, IReturn<LessonResponse>
        {
            public int Id { get; set; }
            public string BookTitle { get; set; }
            public string BookAmazonUrl { get; set; }
            public string TheHookYouTubeVideo { get; set; }
            public string TheTwoVocabularyWordsYouTubeVideo { get; set; }
            public string EnunciationVideo1 { get; set; }
            public string EnunciationVideo2 { get; set; }
            public string MainIdea { get; set; }
            public string SupportingIdea { get; set; }
            public string StoryDetails { get; set; }
            public string StoryQuestions { get; set; }
            public string ImportantSentencesForWordScramble { get; set; }
            
            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
        }

        [Route("/lesson/delete", "DELETE")]
        public class LessonDeleteRequest : IProviderRequest, IReturnVoid
        {
            [ApiMember(IsRequired = true)] public int Id { get; set; }

            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
        }

        // ReSharper disable once UnusedMember.Global
        public IEnumerable<LessonResponse> Post(LessonGetAllRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);

            var allLessons = LessonRepository.GetAllNonDeleted();
            var allAuthors = allLessons.DistinctBy(al => new {al.Provider, al.ProviderId}).Select(au => UserRepository.GetUserByProviderAndProviderId(au.Provider, au.ProviderId));

            return allLessons
                .Select(lesson => new { Grade = existingUser.IsStudent ? GradeRepository.GetGrade(lesson.Id, request.Provider, request.ProviderId) : new GradeModel(), Lesson = lesson })
                .Select(x => new LessonResponse // TODO: Use Automapper
            {
                Id = x.Lesson.Id,
                BookTitle = x.Lesson.BookTitle,
                BookAmazonUrl = x.Lesson.BookAmazonUrl,
                TheHookYouTubeVideo = x.Lesson.TheHookYouTubeVideo,
                TheTwoVocabularyWordsYouTubeVideo = x.Lesson.TheTwoVocabularyWordsYouTubeVideo,
                EnunciationVideo1 = x.Lesson.EnunciationVideo1,
                EnunciationVideo2 = x.Lesson.EnunciationVideo2,
                MainIdea = x.Lesson.MainIdea,
                SupportingIdea = x.Lesson.SupportingIdea,
                StoryDetails = x.Lesson.StoryDetails,
                StoryQuestions = x.Lesson.StoryQuestions,
                ImportantSentencesForWordScramble = x.Lesson.ImportantSentencesForWordScramble,
                LessonAuthor = allAuthors.Single(aa => aa.Provider == x.Lesson.Provider && aa.ProviderId == x.Lesson.ProviderId),
                NumberOfEnrolledStudents = x.Lesson.NumberOfEnrolledStudents,
                GradeResponse = x.Grade == null ? new GradeResponse() : new GradeResponse { IsGraded = true, Grade = x.Grade.Grade, Comments = x.Grade.Comments }
            });
        }

        // ReSharper disable once UnusedMember.Global
        public LessonResponse Post(LessonUpsertRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);
            Guard.IsTrue(eu => eu.IsTeacher, existingUser);

            var lesson = new Lesson
            {
                Id = request.Id,
                BookTitle = request.BookTitle,
                BookAmazonUrl = request.BookAmazonUrl,
                TheHookYouTubeVideo = request.TheHookYouTubeVideo,
                TheTwoVocabularyWordsYouTubeVideo = request.TheTwoVocabularyWordsYouTubeVideo,
                EnunciationVideo1 = request.EnunciationVideo1,
                EnunciationVideo2 = request.EnunciationVideo2,
                MainIdea = request.MainIdea,
                SupportingIdea = request.SupportingIdea,
                StoryDetails = request.StoryDetails,
                StoryQuestions = request.StoryQuestions,
                ImportantSentencesForWordScramble = request.ImportantSentencesForWordScramble,
                Provider = request.Provider,
                ProviderId = request.ProviderId
            };

            if (lesson.IsNew)
            {
                LessonRepository.Add(lesson);
            }
            else
            {
                if (request.Provider != lesson.Provider || request.ProviderId != lesson.ProviderId)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                if (lesson.IsDeleted)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "NotFound");
                }

                lesson.UpdateDateUtc = DateTime.UtcNow;
                LessonRepository.Update(lesson);
            }

            // TODO: Use Automapper
            return new LessonResponse
            {
                Id = lesson.Id,
                BookTitle = lesson.BookTitle,
                BookAmazonUrl = lesson.BookAmazonUrl,
                TheHookYouTubeVideo = lesson.TheHookYouTubeVideo,
                TheTwoVocabularyWordsYouTubeVideo = lesson.TheTwoVocabularyWordsYouTubeVideo,
                EnunciationVideo1 = lesson.EnunciationVideo1,
                EnunciationVideo2 = lesson.EnunciationVideo2,
                MainIdea = lesson.MainIdea,
                SupportingIdea = lesson.SupportingIdea,
                StoryDetails = lesson.StoryDetails,
                StoryQuestions = lesson.StoryQuestions,
                ImportantSentencesForWordScramble = lesson.ImportantSentencesForWordScramble,
                LessonAuthor = existingUser
            };
        }

        // ReSharper disable once UnusedMember.Global
        public void Delete(LessonDeleteRequest request)
        {
            Guard.IsTrue(id => 0 < id, request.Id);
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);
            
            var lesson = LessonRepository.Find(request.Id);
            if (lesson == null)
            {
                return;
            }

            if (existingUser.IsTeacher == false || request.Provider != lesson.Provider || request.ProviderId != lesson.ProviderId)
            {
                throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (lesson.IsDeleted)
            {
                return;
            }
            
            LessonRepository.DeleteLesson(request.Id);
        }
    }
}