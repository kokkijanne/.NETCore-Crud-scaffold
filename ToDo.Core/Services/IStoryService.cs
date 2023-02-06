using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Data.Entities;

namespace Todo.Core.Services
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryWithoutTasksDto>> GetAllStories();
        Task<dynamic> GetStory(int id, bool includeTasks);
        Task<StoryDto> Create(StoryDto story);
        Task<StoryDto> Delete(int id);
        Task<StoryDto> Update(StoryDto story);
    }
}
