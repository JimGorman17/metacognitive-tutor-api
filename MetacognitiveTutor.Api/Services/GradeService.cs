using System;
using System.Net;
using MetacognitiveTutor.Api.Helpers;
using MetacognitiveTutor.Api.Interfaces;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace MetacognitiveTutor.Api.Services
{
    public class GradeService : Service
    {
        public UserRepository UserRepository { get; set; }
        public GradeRepository GradeRepository { get; set; }
        public LessonRepository LessonRepository { get; set; }

        [Route("/grade/upsert", "POST")]
        public class GradeUpsertRequest : IProviderRequest, IReturnVoid
        {
            [ApiMember(IsRequired = true)] public int LessonId { get; set; }
            [ApiMember(IsRequired = true)] public string StudentProvider { get; set; }
            [ApiMember(IsRequired = true)] public string StudentProviderId { get; set; }
            [ApiMember(IsRequired = true, Description = "Refers to the teacher submitting the grade.")] public string Provider { get; set; }
            [ApiMember(IsRequired = true, Description = "Refers to the teacher submitting the grade.")] public string ProviderId { get; set; }
            [ApiMember(IsRequired = true)] public string Grade { get; set; }
            public string Comments { get; set; }
        }

        [Route("/grade/delete", "DELETE")]
        public class LessonDeleteRequest : IProviderRequest, IReturnVoid
        {
            [ApiMember(IsRequired = true)] public int LessonId { get; set; }
            [ApiMember(IsRequired = true)] public string StudentProvider { get; set; }
            [ApiMember(IsRequired = true)] public string StudentProviderId { get; set; }
            [ApiMember(IsRequired = true, Description = "Refers to the teacher submitting the grade.")] public string Provider { get; set; }
            [ApiMember(IsRequired = true, Description = "Refers to the teacher submitting the grade.")] public string ProviderId { get; set; }
        }

        // ReSharper disable once UnusedMember.Global
        public void Post(GradeUpsertRequest request)
        {
            Guard.IsTrue(lessonId => 0 < lessonId, request.LessonId);
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);

            Guard.AgainstEmpty(request.StudentProvider);
            Guard.AgainstEmpty(request.StudentProviderId);
            var existingStudent = UserHelpers.GetExistingUser(new LessonDeleteRequest { Provider = request.StudentProvider, ProviderId = request.StudentProviderId }, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingStudent);
            Guard.IsTrue(es => es.IsStudent, existingStudent);

            var lesson = LessonRepository.Find(request.LessonId);
            if (lesson == null || lesson.IsDeleted)
            {
                throw new HttpError(HttpStatusCode.NotFound, "NotFound");
            }

            if (request.Provider != lesson.Provider || request.ProviderId != lesson.ProviderId)
            {
                throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            var grade = GradeRepository.GetGrade(request.LessonId, request.StudentProvider, request.ProviderId);
            if (grade == null)
            {
                grade = new GradeModel
                {
                    LessonId = request.LessonId,
                    StudentProvider = request.StudentProvider,
                    StudentProviderId = request.StudentProviderId,
                    TeacherProvider = request.Provider,
                    TeacherProviderId = request.ProviderId,
                    Grade = request.Grade,
                    Comments = request.Comments
                };

                GradeRepository.Add(grade);
            }
            else
            {
                grade.LessonId = request.LessonId;
                grade.StudentProvider = request.StudentProvider;
                grade.StudentProviderId = request.StudentProviderId;
                grade.TeacherProvider = request.Provider;
                grade.TeacherProviderId = request.ProviderId;
                grade.Grade = request.Grade;
                grade.Comments = request.Comments;
                grade.UpdateDateUtc = DateTime.UtcNow;

                GradeRepository.Update(grade);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void Delete(LessonDeleteRequest request)
        {
            Guard.IsTrue(lessonId => 0 < lessonId, request.LessonId);
            Guard.AgainstEmpty(request.Provider);
            Guard.AgainstEmpty(request.ProviderId);
            var existingUser = UserHelpers.GetExistingUser(request, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingUser);

            Guard.AgainstEmpty(request.StudentProvider);
            Guard.AgainstEmpty(request.StudentProviderId);
            var existingStudent = UserHelpers.GetExistingUser(new LessonDeleteRequest {Provider = request.StudentProvider, ProviderId = request.StudentProviderId }, UserRepository);
            Guard.IsTrue(eu => eu.IsNew == false, existingStudent);
            Guard.IsTrue(es => es.IsStudent, existingStudent);

            var grade = GradeRepository.GetGrade(request.LessonId, request.StudentProvider, request.ProviderId);
            if (grade == null)
            {
                return;
            }

            if (existingUser.IsTeacher == false || request.Provider != grade.TeacherProvider || request.ProviderId != grade.TeacherProviderId)
            {
                throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            if (grade.IsDeleted)
            {
                return;
            }

            GradeRepository.DeleteGrade(grade.Id);
        }
    }
}