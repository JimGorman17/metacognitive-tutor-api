using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetacognitiveTutor.Api.Dtos
{
    public class GroupedStudentLessonAnswerResponse
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderPic { get; set; }
        public IEnumerable<StudentLessonAnswerResponse> StudentLessonAnswers { get; set; }
    }
}