using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class LessonService : Service
    {
        public Repository<Lesson> LessonRepository { get; set; }

        [Route("/lesson/create", "POST")]
        public class LessonCreateRequest : IProviderRequest, IReturn<int>
        {
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

        [Route("/lesson/update", "PUT")]
        public class LessonUpdateRequest : IProviderRequest, IReturnVoid
        {
            [ApiMember(IsRequired = true)] public int Id { get; set; }
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
    }
}