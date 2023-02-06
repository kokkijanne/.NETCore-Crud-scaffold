using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Core.Repositories;
using Todo.Data.Entities;


namespace Todo.Core.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository storyRepository;
        private readonly IMapper mapper;

        public StoryService(
            IMapper mapper,
            IStoryRepository storyRepository)
        {
            this.mapper = mapper;
            this.storyRepository = storyRepository;
        }

        public async Task<IEnumerable<StoryWithoutTasksDto>> GetAllStories()
        {

            var stories = await this.storyRepository.GetAll();
            var storyDtos = new List<StoryWithoutTasksDto>();
            foreach (var story in stories)
            {
                var dto = mapper.Map<StoryWithoutTasksDto>(story);

                storyDtos.Add(dto);
            }

            return storyDtos;
        }


        public async Task<dynamic> GetStory(int id, bool includeTasks)
        {
            var story = await this.storyRepository.GetStory(id);
            dynamic result = null;

            if (includeTasks)
            {
              result =  this.mapper.Map<StoryDto>(story);
            } else {
              result = this.mapper.Map<StoryWithoutTasksDto>(story);
            }
            return result;
        }

        public async Task<StoryDto> Create(StoryDto story)
        {
            var storyAsEntity = mapper.Map<StoryEntity>(story);
            var createdEntity = await this.storyRepository.Create(storyAsEntity);
            var result = mapper.Map<StoryDto>(createdEntity);
            return result;
        }

        public async Task<StoryDto> Delete(int id)
        {

            var result = await this.storyRepository.Delete(id);
            return mapper.Map<StoryDto>(result);
        }

        public async Task<StoryDto> Update(StoryDto story)
        {
            var storyAsEntity = this.mapper.Map<StoryEntity>(story);
            var result = await this.storyRepository.Update(storyAsEntity);
            return mapper.Map<StoryDto>(result);

        }
    }
}
