using Polly;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TaskEngine.Domain.Entities;
using TaskEngine.Domain.Entities.Constants;
using TaskEngine.Domain.Interfaces.Repository;
using TaskEngine.Domain.Interfaces.Service;

namespace TaskEngine.Domain.Service
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IModel _channel;

        public JobService(IJobRepository jobRepository, IModel channel)
        {
            _jobRepository = jobRepository;
            _channel = channel;
        }

        public async Task CreateJobAsync(Job job)
        {
            await _jobRepository.AddAsync(job);
        }

        public async Task<Job?> GetJobByIdAsync(Guid id)
        {
            return await _jobRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _jobRepository.GetAllAsync();
        }

        public async Task UpdateJobAsync(Job job)
        {
            await _jobRepository.UpdateAsync(job.Id, job);
        }

        public async Task DeleteJobAsync(Guid id)
        {
            await _jobRepository.DeleteAsync(id);
        }

        public async Task SendToQueue(Guid id)
        {
            var job = await _jobRepository.GetByIdAsync(id);

            if (job != null)
            {
                var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"[SendToQueue] Attempt {retryCount}: Retrying in {timeSpan.TotalSeconds} seconds due to error: {exception.Message}");
                });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        var message = JsonSerializer.Serialize(job);
                        var body = Encoding.UTF8.GetBytes(message);

                        _channel.BasicPublish(
                        exchange: "",
                        routingKey: job.Name,
                        basicProperties: null,
                        body: body
                    );

                        Console.WriteLine($"[SendToQueue] Job sent to queue: {job.Name}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SendToQueue] {job.Name} - Error sending job to queue: {ex.Message}");
                        throw;
                    }
                });
            }
            else
                Console.WriteLine("[SendToQueue] Job not found.");
        }

        #region :: Process Services ::
        public async Task ProcessEnviarEmail(Job job)
        {
            try
            {
                UpdateStatusJob(job, Constants.JOB_PROCESSING,"");
                _jobRepository.UpdateAsync(job.Id, job);

                //Process fake.
                await Task.Delay(2000);

                UpdateStatusJob(job, Constants.JOB_COMPLETED, "");
                _jobRepository.UpdateAsync(job.Id, job);
            }
            catch (Exception ex)
            {
                UpdateStatusJob(job, Constants.JOB_FAILED, ex.Message);
                _jobRepository.UpdateAsync(job.Id, job);
                throw ex;
            }
        }
        public async Task ProcessEnviarNotificacao(Job job)
        {
            try
            {
                UpdateStatusJob(job, Constants.JOB_PROCESSING, "");
                _jobRepository.UpdateAsync(job.Id, job);

                //Process fake.
                await Task.Delay(2000);

                UpdateStatusJob(job, Constants.JOB_COMPLETED, "");
                _jobRepository.UpdateAsync(job.Id, job);
            }
            catch (Exception ex)
            {
                UpdateStatusJob(job, Constants.JOB_FAILED, ex.Message);
                _jobRepository.UpdateAsync(job.Id, job);
                throw ex;
            }
        }

        public async Task ProcessProcessarFatura(Job job)
        {
            try
            {
                UpdateStatusJob(job, Constants.JOB_PROCESSING, "");
                _jobRepository.UpdateAsync(job.Id, job);

                //Process fake.
                await Task.Delay(2000);

                UpdateStatusJob(job, Constants.JOB_COMPLETED, "");
                _jobRepository.UpdateAsync(job.Id, job);
            }
            catch (Exception ex)
            {
                UpdateStatusJob(job, Constants.JOB_FAILED, ex.Message);
                _jobRepository.UpdateAsync(job.Id, job);
                throw ex;
            }
        }

        public async Task ProcessProcessarPagamento(Job job)
        {
            try
            {
                UpdateStatusJob(job, Constants.JOB_PROCESSING, "");
                _jobRepository.UpdateAsync(job.Id, job);

                //Process fake.
                await Task.Delay(2000);

                UpdateStatusJob(job, Constants.JOB_COMPLETED, "");
                _jobRepository.UpdateAsync(job.Id, job);
            }
            catch (Exception ex)
            {
                UpdateStatusJob(job, Constants.JOB_FAILED, ex.Message);
                _jobRepository.UpdateAsync(job.Id, job);
                throw ex;
            }
        }

        public async Task ProcessProcessarReconciliacao(Job job)
        {
            try
            {
                UpdateStatusJob(job, Constants.JOB_PROCESSING, "");
                _jobRepository.UpdateAsync(job.Id, job);

                //Process fake.
                await Task.Delay(2000);

                UpdateStatusJob(job, Constants.JOB_COMPLETED, "");
                _jobRepository.UpdateAsync(job.Id, job);
            }
            catch (Exception ex)
            {
                UpdateStatusJob(job, Constants.JOB_FAILED, ex.Message);
                _jobRepository.UpdateAsync(job.Id, job);
                throw ex;
            }
        }

        private void UpdateStatusJob(Job job, string status, string message)
        {
            try
            {
                job.Status = status;
                job.Message = message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateStatusJob] error Id {job.Id} status {status} message {message}.");
                throw ex;
            }
        }

        #endregion
    }
}
