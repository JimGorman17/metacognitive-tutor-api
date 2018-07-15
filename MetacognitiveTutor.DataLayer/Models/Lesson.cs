using System;
using MetacognitiveTutor.DataLayer.Interfaces;
using PetaPoco;

namespace MetacognitiveTutor.DataLayer.Models
{
    [TableName("Lessons")]
    [PrimaryKey("Id")]
    public class Lesson : IEntity
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
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        // [ResultColumn] public DateTime CreateDateUtc { get; set; } // Not a UI concern for now.
        public DateTime? UpdateDateUtc { get; set; }
        public bool IsDeleted { get; set; }

        [Ignore]
        public bool IsNew => Id == default(int);
    }
}
