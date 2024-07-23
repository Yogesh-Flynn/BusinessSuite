using BusinessSuite.Models;
using BusinessSuite.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BusinessSuite.Controllers
{
    public class JobController : ControllerBase
    {
        
        private readonly MyJobService _jobService;

        public JobController(MyJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("schedule")]
        public IActionResult ScheduleJob(Message request)
        {
            // Schedule a job to store data at the specified time
            BackgroundJob.Schedule(() => _jobService.StoreDataAsync(request.PhoneNumber,request.MessageText,request.Status, DateTime.Now), request.ScheduleTime - DateTime.Now);

            return Ok("Job scheduled successfully");
        }
    }
}
