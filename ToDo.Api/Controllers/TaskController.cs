using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Api.Middleware;
using Todo.Core.Models;
using Todo.Core.Services;

namespace Todo.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {

        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger, ITaskService _taskService)
        {
            this._logger = logger;
            this._taskService = _taskService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTasks()
        {
            this._logger.LogDebug($"GET api/tasks");
            var allTasks = await this._taskService.GetAllTasks();
            return this.Ok(allTasks);

        }

        [CheckPinCode]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskDto task)
        {
            this._logger.LogDebug($"POST api/Tasks");

            await this._taskService.Create(task);
            return this.Ok(task);
        }

        [CheckPinCode]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            this._logger.LogDebug($"DELETE api/tasks/{id}");

            var result = await this._taskService.Delete(id);

            if (result == null)
            {
                return this.NotFound();
            }
            return this.NoContent();
        }

        [CheckPinCode]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] TaskDto task, int id)
        {
            if (task == null)
            {
                return this.BadRequest();
            }

            this._logger.LogDebug($"PUT api/tasks/{id}");
            task.Id = id;

            var result = await this._taskService.Update(task);

            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

    }
}
