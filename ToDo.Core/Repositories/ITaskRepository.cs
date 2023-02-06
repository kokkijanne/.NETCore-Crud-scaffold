using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;

namespace Todo.Core.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAll();

        Task<TaskEntity> Create(TaskEntity Story);

        Task<TaskEntity> Delete(int Id);

         Task<TaskEntity> Update(TaskEntity task);

    }
}
