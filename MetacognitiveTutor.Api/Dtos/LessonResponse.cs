using MetacognitiveTutor.DataLayer.Models;

namespace MetacognitiveTutor.Api.Dtos
{
    public class LessonResponse
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string BookAmazonUrl { get; set; }
        public string TheHookYouTubeVideo { get; set; }
        public string TheTwoVocabularyWordsYouTubeVideo { get; set; }
        public string MainIdea { get; set; }
        public string SupportingIdea { get; set; }
        public string StoryDetails { get; set; }
        public string StoryQuestions { get; set; }
        public string ImportantSentencesForWordScramble { get; set; }
        public User LessonAuthor { get; set; }
        public int NumberOfEnrolledStudents { get; set; }
    }
}