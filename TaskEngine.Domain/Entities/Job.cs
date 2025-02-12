using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskEngine.Domain.Entities
{
    public class Job
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public EJobType JobType {get;set;}
        public string Name { get; set; }
        public string Data { get; set; }
        public string Status { get; set; } = Constants.Constants.JOB_PENDING;
        public string Message { get; set; } = string.Empty;
    }

    public enum EJobType
    {
        None,
        EnviarEmail,
        EnviarNotificacao,
        ProcessarFatura,
        ProcessarPagamento,
        ProcessarReconciliacao
    }
}
