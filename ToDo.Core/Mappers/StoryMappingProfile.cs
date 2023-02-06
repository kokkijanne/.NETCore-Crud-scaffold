using AutoMapper;
using Todo.Core.Models;
using Todo.Data.Entities;

namespace Todo.Core.Mappers
{
    public class StoryMappingProfile : Profile
    {
        public StoryMappingProfile()
        {
            CreateMap<StoryDto, StoryEntity>();
            CreateMap<StoryEntity, StoryDto>();

            CreateMap<StoryEntity, StoryWithoutTasksDto>().ForMember(x=>x.TaskCount, opt => opt.MapFrom(entity => entity.Tasks.Count));
        }
    }
}
