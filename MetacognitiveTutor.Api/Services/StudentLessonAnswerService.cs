using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MetacognitiveTutor.Api.Dtos;
using MetacognitiveTutor.Api.Helpers;
using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class StudentLessonAnswerService : Service
    {
        public UserRepository UserRepository { get; set; }
        public StudentLessonAnswerRepository StudentLessonAnswerRepository { get; set; }
        public GradeRepository GradeRepository { get; set; }
        public LessonRepository LessonRepository { get; set; }
        public Repository<ErrorLog> ErrorLogRepository { get; set; }

        [Route("/studentlessonanswer/for-student/getall", "POST")]
        public class StudentLessonAnswerGetRequest : IProviderRequest, IReturn<IEnumerable<StudentLessonAnswerResponse>>
        {
            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
        }

        [Route("/studentlessonanswer/upsert", "POST")]
        public class StudentLessonAnswerUpsertRequest : IProviderRequest, IReturn<StudentLessonAnswerResponse>
        {
            public int Id { get; set; }
            [ApiMember(IsRequired = true)] public int LessonId { get; set; }
            [ApiMember(IsRequired = true)] public string QuestionType { get; set; }
            public int QuestionId { get; set; }
            public string Question { get; set; }
            public string Answer { get; set; }

            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
        }

        [Route("/studentlessonanswer/for-teacher/get-all-by-lessonid", "POST")]
        public class StudentLessonAnswerGetAllRequest : IProviderRequest, IReturn<IEnumerable<GroupedStudentLessonAnswerResponse>>
        {
            [ApiMember(IsRequired = true)] public string Provider { get; set; }
            [ApiMember(IsRequired = true)] public string ProviderId { get; set; }
            [ApiMember(IsRequired = true)] public int LessonId { get; set; }
        }

        // ReSharper disable once UnusedMember.Global
        public StudentLessonAnswerResponse Post(StudentLessonAnswerUpsertRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            Guard.IsTrue(li => 0 < li, request.LessonId);
            Guard.AgainstEmpty(request.QuestionType);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);
            Guard.IsTrue(eu => eu.IsStudent, existingUser);

            var studentLessonAnswer = new StudentLessonAnswer
            {
                Id = request.Id,
                Provider = request.Provider,
                ProviderId = request.ProviderId,
                LessonId = request.LessonId,
                QuestionType = request.QuestionType,
                QuestionId = request.QuestionId,
                Question = request.Question,
                Answer = request.Answer
            };             

            if (studentLessonAnswer.IsNew)
            {
                StudentLessonAnswerRepository.Add(studentLessonAnswer);
            }
            else
            {
                if (request.Provider != studentLessonAnswer.Provider || request.ProviderId != studentLessonAnswer.ProviderId)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                studentLessonAnswer.UpdateDateUtc = DateTime.UtcNow;
                StudentLessonAnswerRepository.Update(studentLessonAnswer);
            }

            // TODO: Use Automapper
            return new StudentLessonAnswerResponse
            {
                Id = studentLessonAnswer.Id,
                LessonId = studentLessonAnswer.LessonId,
                QuestionType = studentLessonAnswer.QuestionType,
                QuestionId = studentLessonAnswer.QuestionId,
                Question = studentLessonAnswer.Question,
                Answer = studentLessonAnswer.Answer,
                Student = existingUser
            };
        }

        // ReSharper disable once UnusedMember.Global
        public IEnumerable<StudentLessonAnswerResponse> Post(StudentLessonAnswerGetRequest request)
        {
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);
            Guard.IsTrue(eu => eu.IsStudent, existingUser);

            var studentLessonAnswers = StudentLessonAnswerRepository.GetAllByProviderAndProviderId(request.Provider, request.ProviderId);
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

            var lesson = LessonRepository.Find(request.LessonId);
            if (request.Provider != lesson.Provider || request.ProviderId != lesson.ProviderId)
            {
                throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
            }

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

                var grade = GradeRepository.GetGrade(request.LessonId, student.Provider, student.ProviderId);
                response.Add(new GroupedStudentLessonAnswerResponse
                {
                    LessonId = request.LessonId,
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
                    }),
                    GradeResponse = grade == null ? new GradeResponse { IsGraded = false } : new GradeResponse { IsGraded = true, Comments = grade.Comments, Grade = grade.Grade }
                });
            }

            return response;
        }
    }
}