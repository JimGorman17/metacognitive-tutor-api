using System;
using MetacognitiveTutor.DataLayer.Interfaces;
using PetaPoco;

namespace MetacognitiveTutor.DataLayer.Models
{
    [TableName("StudentLessonAnswers")]
    [PrimaryKey("Id")]
    public class StudentLessonAnswer : IEntity
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public int LessonId { get; set; }
        public string QuestionType { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        // [ResultColumn] public DateTime CreateDateUtc { get; set; } // Not a UI concern for now.
        public DateTime? UpdateDateUtc { get; set; }

        [Ignore]
        public bool IsNew => Id == default(int);
    }
}
