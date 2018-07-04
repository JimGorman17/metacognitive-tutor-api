using System;
using MetacognitiveTutor.DataLayer.Interfaces;
using PetaPoco;

namespace MetacognitiveTutor.DataLayer.Models
{
    [TableName("ErrorLogs")]
    [PrimaryKey("Id")]
    public class ErrorLog : IEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreateDateUtc { get; set; }
        public int? UserId { get; set; }

        [Ignore]
        public bool IsNew => Id == default(int);
    }
}
