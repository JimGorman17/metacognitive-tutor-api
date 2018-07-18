using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MetacognitiveTutor.Api.Dtos;
using MetacognitiveTutor.Api.Helpers;
using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class StudentLessonAnswerService : Service
    {
        public UserRepository UserRepository { get; set; }
        public StudentLessonAnswerRepository StudentLessonAnswerRepository { get; set; }
        public Repository<ErrorLog> ErrorLogRepository { get; set; }

        [Route("/studentlessonanswer/get", "POST")]
        public class StudentLessonAnswerGetRequest : IProviderRequest, IReturn<IEnumerable<StudentLessonAnswerResponse>>
        {
            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
            [ApiMember(IsRequired = true)] public int LessonId { get; set; }
        }

        [Route("/studentlessonanswer/getall", "POST")]
        public class StudentLessonAnswerGetAllRequest : IProviderRequest, IReturn<IEnumerable<StudentLessonAnswerResponse>>
        {
            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
            [ApiMember(IsRequired = true)] public int LessonId { get; set; }
        }

        // ReSharper disable once UnusedMember.Global
        public IEnumerable<StudentLessonAnswerResponse> Post(StudentLessonAnswerGetRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);
            Guard.GreaterThan(0, request.LessonId, "LessonId");
            Guard.IsTrue(eu => eu.IsStudent, existingUser);

            var studentLessonAnswers = StudentLessonAnswerRepository.GetAllByProviderAndProviderIdAndLessonId(request.Provider, request.ProviderId, request.LessonId);
            return studentLessonAnswers.Select(studentLessonAnswer => new StudentLessonAnswerResponse // TODO: Use Automapper
            {
                Id = studentLessonAnswer.Id,
                LessonId = studentLessonAnswer.LessonId,
                QuestionType = studentLessonAnswer.QuestionType,
                QuestionId = studentLessonAnswer.QuestionId,
                Question = studentLessonAnswer.Question,
                Answer = studentLessonAnswer.Answer
            });
        }

        // ReSharper disable once UnusedMember.Global
        public IEnumerable<GroupedStudentLessonAnswerResponse> Post(StudentLessonAnswerGetAllRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);
            Guard.GreaterThan(0, request.LessonId, "LessonId");
            Guard.IsTrue(eu => eu.IsTeacher, existingUser);

            var studentLessonAnswers = StudentLessonAnswerRepository.GetAllByLessonId(request.LessonId);
            var groupedStudentLessonAnswers = studentLessonAnswers.GroupBy(g => new { g.Provider, g.ProviderId }).ToList();
            var allStudents = groupedStudentLessonAnswers.Select(g => UserRepository.GetUserByProviderAndProviderId(g.Key.Provider, g.Key.ProviderId)).ToList();

            var response = new List<GroupedStudentLessonAnswerResponse>();
            foreach (var grouping in groupedStudentLessonAnswers)
            {
                var student = allStudents.SingleOrDefault(a => a.Provider == grouping.Key.Provider && a.ProviderId == grouping.Key.ProviderId);
                if (student == null || student.IsStudent == false)
                {
                    ErrorLogRepository.Add(new ErrorLog
                    {
                        Application = "MetacognitiveTutor.Api",
                        Message = $"No student found for Provider '{grouping.Key.Provider}', ProviderId '{grouping.Key.ProviderId}'; LessonId: '{request.LessonId}'.",
                        Provider = request.Provider,
                        ProviderId = request.ProviderId,
                    });
                    continue;
                }

                response.Add(new GroupedStudentLessonAnswerResponse
                {
                    Name = student.Name,
                    Provider = student.Provider,
                    ProviderId = student.ProviderId,
                    ProviderPic = student.ProviderPic,
                    StudentLessonAnswers = grouping.Select(g => new StudentLessonAnswerResponse
                    {
                        Id = g.Id,
                        LessonId = g.LessonId,
                        QuestionType = g.QuestionType,
                        QuestionId = g.QuestionId,
                        Question = g.Question,
                        Answer = g.Answer
                    })
                });
            }

            return response;
        }
    }
}