using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEngine.Domain.Entities;

namespace TaskEngine.Domain.Interfaces.Service
{
    public interface IJobService
    {
        Task CreateJobAsync(Job job);
        Task<Job?> GetJobByIdAsync(Guid id);
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(Guid id);
        Task SendToQueue(Guid id);

        #region :: Process Service ::
        Task ProcessEnviarEmail(Job job);
        Task ProcessEnviarNotificacao(Job job);
        Task ProcessProcessarFatura(Job job);
        Task ProcessProcessarPagamento(Job job);
        Task ProcessProcessarReconciliacao(Job job);
        #endregion
    }
}
