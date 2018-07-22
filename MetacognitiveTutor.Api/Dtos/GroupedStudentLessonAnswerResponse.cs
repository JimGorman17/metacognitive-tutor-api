using System.Collections.Generic;

namespace MetacognitiveTutor.Api.Dtos
{
    public class GroupedStudentLessonAnswerResponse
    {
        public int LessonId { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderPic { get; set; }
        public IEnumerable<StudentLessonAnswerResponse> StudentLessonAnswers { get; set; }
        public GradeResponse GradeResponse { get; set; }
    }
}