using MetacognitiveTutor.DataLayer.Models;

namespace MetacognitiveTutor.DataLayer.Dtos
{
    public class LessonDto : Lesson
    {
        public int NumberOfEnrolledStudents { get; set; }
    }
}
