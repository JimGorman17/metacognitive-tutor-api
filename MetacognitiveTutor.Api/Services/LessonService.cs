using System;
using System.Net;
using MetacognitiveTutor.Api.Helpers;
using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class LessonService : Service
    {
        public UserRepository UserRepository { get; set; }
        public LessonRepository LessonRepository { get; set; }

        [Route("/lesson/upsert", "POST")]
        public class LessonUpsertRequest : IProviderRequest, IReturn<int>
        {
            public int Id { get; set; }
            public string BookTitle { get; set; }
            public string BookAmazonUrl { get; set; }
            public string TheHookYouTubeVideo { get; set; }
            public string TheTwoVocabularyWordsYouTubeVideo { get; set; }
            public string MainIdea { get; set; }
            public string SupportingIdea { get; set; }
            public string StoryDetails { get; set; }
            public string ImportantSentencesForWordScramble { get; set; }
            public string LessonAuthorProvider { get; set; }
            public string LessonAuthorProviderId { get; set; }

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
        public int Post(LessonUpsertRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);

            var lesson = new Lesson
            {
                Id = request.Id,
                BookTitle = request.BookTitle,
                BookAmazonUrl = request.BookAmazonUrl,
                TheHookYouTubeVideo = request.TheHookYouTubeVideo,
                TheTwoVocabularyWordsYouTubeVideo = request.TheTwoVocabularyWordsYouTubeVideo,
                MainIdea = request.MainIdea,
                SupportingIdea = request.SupportingIdea,
                StoryDetails = request.StoryDetails,
                ImportantSentencesForWordScramble = request.ImportantSentencesForWordScramble,
                LessonAuthorProvider = request.LessonAuthorProvider,
                LessonAuthorProviderId = request.LessonAuthorProviderId
            };

            if (lesson.IsNew)
            {
                var newLesson = LessonRepository.Add(lesson);
                return newLesson.Id;
            }
            else
            {
                if (request.Provider != lesson.LessonAuthorProvider || request.ProviderId != lesson.LessonAuthorProviderId)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                if (lesson.IsDeleted)
                {
                    throw new HttpError(HttpStatusCode.NotFound, "NotFound");
                }

                lesson.UpdateDateUtc = DateTime.UtcNow;
                LessonRepository.Update(lesson);
                return lesson.Id;
            }
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

            if (request.Provider != lesson.LessonAuthorProvider || request.ProviderId != lesson.LessonAuthorProviderId)
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