using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskEngine.Domain.Entities.Constants
{
    public static class Constants
    {
        public const string JOB_PENDING = "Pendente";
        public const string JOB_PROCESSING = "EmProcessamento";
        public const string JOB_COMPLETED = "Concluido";
        public const string JOB_FAILED = "Erro";
    }
}
