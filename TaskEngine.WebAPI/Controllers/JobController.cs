using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using TaskEngine.Domain.Entities;
using TaskEngine.Domain.Interfaces.Service;

namespace TaskEngine.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        #region :: CRUD ::

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Job job)
        {
            if (job == null)
                return BadRequest("Job is Invalid");

            await _jobService.CreateJobAsync(job);
            _jobService.SendToQueue(job.Id);

            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound("Job not found.");
            }

            return Ok(job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Job updatedJob)
        {
            if (updatedJob == null)
            {
                return BadRequest("Job is Invalid.");
            }

            var existingJob = await _jobService.GetJobByIdAsync(id);
            if (existingJob == null)
            {
                return NotFound("Job not found.");
            }

            updatedJob.Id = existingJob.Id; // Garante que o ID não será alterado
            await _jobService.UpdateJobAsync(updatedJob);
            return Ok(updatedJob);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound("Job not found.");
            }

            await _jobService.DeleteJobAsync(id);
            return NoContent();
        }

        [HttpPost("SendToQueue/{id}")]
        public async Task<IActionResult> SendToQueue(Guid id)
        {
            try
            {
                await _jobService.SendToQueue(id);
                return Ok("Published to Queue.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[SendToQueue] Error: {ex.Message}");
            }
        }


        #endregion

        #region :: PROCESS ROUTES ::

        [HttpPost("EnviarEmail")]
        public async Task<IActionResult> EnviarEmail([FromBody] string request)
        {
            try
            {
                if (String.IsNullOrEmpty(request))
                    return BadRequest("Job is Invalid");

                var job = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(request);
                await _jobService.ProcessEnviarEmail(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[EnviarEmail] Error in process - {ex.Message}");
            }
        }

        [HttpPost("EnviarNotificacao")]
        public async Task<IActionResult> EnviarNotificacao([FromBody] string request)
        {
            try
            {
                if (String.IsNullOrEmpty(request))
                    return BadRequest("Job is Invalid");

                var job = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(request);
                await _jobService.ProcessEnviarNotificacao(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[EnviarNotificacao] Error in process - {ex.Message}");
            }
        }

        [HttpPost("ProcessarFatura")]
        public async Task<IActionResult> ProcessarFatura([FromBody] string request)
        {
            try
            {
                if (String.IsNullOrEmpty(request))
                    return BadRequest("Job is Invalid");

                var job = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(request);
                await _jobService.ProcessProcessarFatura(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[ProcessarFatura] Error in process - {ex.Message}");
            }
        }
        
        [HttpPost("ProcessarPagamento")]
        public async Task<IActionResult> ProcessarPagamento([FromBody] string request)
        {
            try
            {
                if (String.IsNullOrEmpty(request))
                    return BadRequest("Job is Invalid");

                var job = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(request);
                await _jobService.ProcessProcessarPagamento(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[ProcessarPagamento] Error in process - {ex.Message}");
            }
        }

        
        [HttpPost("ProcessarReconciliacao")]
        public async Task<IActionResult> ProcessarReconciliacao([FromBody] string request)
        {
            try
            {
                if (String.IsNullOrEmpty(request))
                    return BadRequest("Job is Invalid");

                var job = Newtonsoft.Json.JsonConvert.DeserializeObject<Job>(request);
                await _jobService.ProcessProcessarReconciliacao(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"[ProcessarReconciliacao] Error in process - {ex.Message}");
            }
        }

        #endregion

    }
}
