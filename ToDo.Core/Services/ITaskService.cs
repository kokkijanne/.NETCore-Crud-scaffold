using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Data.Entities;

namespace Todo.Core.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasks();
        Task<TaskDto> Create(TaskDto task);
        Task<TaskDto> Delete(int id);
        Task<TaskDto> Update(TaskDto task);


    }
}
