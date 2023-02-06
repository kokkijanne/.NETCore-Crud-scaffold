using AutoMapper;
using Todo.Core.Models;
using Todo.Data.Entities;

namespace Todo.Core.Mappers
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            this.CreateMap<TaskEntity, TaskDto>();
            this.CreateMap<TaskDto, TaskEntity>();
        }
    }
}
