using System;
using MetacognitiveTutor.DataLayer.Interfaces;
using PetaPoco;

namespace MetacognitiveTutor.DataLayer.Models
{
    [TableName("Grades")]
    [PrimaryKey("Id")]
    public class GradeModel : IEntity
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string StudentProvider { get; set; }
        public string StudentProviderId { get; set; }
        public string TeacherProvider { get; set; }
        public string TeacherProviderId { get; set; }
        public string Grade { get; set; }
        public string Comments { get; set; }
        // [ResultColumn] public DateTime CreateDateUtc { get; set; } // Not a UI concern for now.
        public DateTime? UpdateDateUtc { get; set; }
        public bool IsDeleted { get; set; }

        [Ignore]
        public bool IsNew => Id == default(int);
    }
}
