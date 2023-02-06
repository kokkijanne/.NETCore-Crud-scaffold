using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Core.Repositories;
using Todo.Data.Entities;

namespace Todo.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IMapper mapper;

        public TaskService(
            IMapper mapper,
            ITaskRepository taskRepository)
        {
            this.mapper = mapper;
            this.taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasks()
        {
            var tasks = await this.taskRepository.GetAll();
            var taskDtos = new List<TaskDto>();
            foreach (var task in tasks)
            {
                var taskDto = this.mapper.Map<TaskEntity, TaskDto>(task);
                taskDtos.Add(taskDto);
            }

            return taskDtos;
        }
        public async Task<TaskDto> Create(TaskDto task)
        {
            var taskAsEntity = this.mapper.Map<TaskEntity>(task);
            var createdEntity = await this.taskRepository.Create(taskAsEntity);
            var result = mapper.Map<TaskDto>(createdEntity);
            return result;
        }

        public async Task<TaskDto> Delete(int id)
        {
            var result = await this.taskRepository.Delete(id);
            return mapper.Map<TaskDto>(result);
        }

        public async Task<TaskDto> Update(TaskDto task)
        {
            var taskAsEntity = this.mapper.Map<TaskEntity>(task);
            var result = await this.taskRepository.Update(taskAsEntity);
            return mapper.Map<TaskDto>(result);

        }
    }
}
