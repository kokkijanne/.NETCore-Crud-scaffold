using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Api.Middleware;
using Todo.Core.Models;
using Todo.Core.Services;

namespace Todo.Api.Controllers
{
    [ApiController]
    [Route("api/stories")]
    public class StoryController : ControllerBase
    {

        private readonly IStoryService _storyService;
        private readonly ILogger<StoryController> _logger;

        public StoryController(ILogger<StoryController> logger, IStoryService _storyService)
        {
            this._logger = logger;
            this._storyService = _storyService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStories()
        {
            this._logger.LogDebug($"GET api/stories");
            var allStories = await this._storyService.GetAllStories();
            return this.Ok(allStories);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStory(int id, bool includeTasks = false)
        {
            this._logger.LogDebug($"GET api/stories");
            var story = await this._storyService.GetStory(id, includeTasks);
            if (story == null)
            {
                return NotFound();
            }

            return this.Ok(story);
        }


        [CheckPinCode]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StoryDto story)
        {
            this._logger.LogDebug($"POST api/story");

            await this._storyService.Create(story);
            return this.Created("api/stories", story);
        }

        [CheckPinCode]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            this._logger.LogDebug($"DELETE api/story");

            var result = await this._storyService.Delete(id);

            if (result == null)
            {
                return this.NotFound();
            }
            return this.NoContent();
        }

        [CheckPinCode]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] StoryDto story, int id)
        {
            if (story == null)
            {
                return this.BadRequest();
            }

            this._logger.LogDebug($"PUT api/story/{id}");
            story.Id = id;

            var result = await this._storyService.Update(story);

            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }
    }
}
