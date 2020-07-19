using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Todo.DomainModels;
using Todo.DomainModels.Exceptions;
using Todo.Services.Interfaces;


namespace Todo.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private IScheduleService _scheduleService;

        public const string GetSchedule = nameof(GetSchedule);
        public const string UpdateSchedule = nameof(UpdateSchedule);
        public const string DeleteSchedule = nameof(DeleteSchedule);

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("{scheduleId}", Name = GetSchedule)]
        public async Task<IActionResult> Get(int scheduleId)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);

            var schedule = await _scheduleService.GetSchedule(userId, scheduleId);

            return Ok(schedule);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);

            var schedules = await _scheduleService.GetSchedulesByUserId(userId);

            return Ok(schedules);
        }

        [HttpPut("{scheduleId}", Name = UpdateSchedule)]
        public async Task<IActionResult> Update(int scheduleId, [FromBody] UpdateScheduleModel updateScheduleModel)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);

            try
            {
                await _scheduleService.UpdateSchedule(userId, scheduleId, updateScheduleModel);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{scheduleId}", Name = DeleteSchedule)]
        public async Task<IActionResult> Delete(int scheduleId)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);

            try
            {
                await _scheduleService.DeleteSchedule(userId, scheduleId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleModel createScheduleModel)
        {
            var userId = int.Parse(HttpContext.User.Identity.Name);

            await _scheduleService.CreateSchedule(userId, createScheduleModel);

            return NoContent();
        }
    }
}
