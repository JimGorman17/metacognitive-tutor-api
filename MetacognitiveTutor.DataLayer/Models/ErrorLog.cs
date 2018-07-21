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
        public string Application { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        // [ResultColumn] public DateTime CreateDateUtc { get; set; } // Not a UI concern for now.
        public string Provider { get; set; }
        public string ProviderId { get; set; }

        [Ignore]
        public bool IsNew => Id == default(int);
    }
}
