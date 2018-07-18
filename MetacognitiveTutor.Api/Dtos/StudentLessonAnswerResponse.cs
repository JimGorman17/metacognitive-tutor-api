namespace MetacognitiveTutor.Api.Dtos
{
    public class StudentLessonAnswerResponse
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string QuestionType { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}