using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Core.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoContext _context;

        public TaskRepository(ToDoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            return await this._context.Tasks.OrderBy(t => t.Name).ToListAsync();
        }

        public async Task<TaskEntity> Create(TaskEntity task)
        {
            this._context.Tasks.Add(task);
            await this._context.SaveChangesAsync();
            return task;
        }


        public async Task<TaskEntity> Delete(int Id)
        {
            var entityToDelete = await this._context.Tasks.FindAsync(Id);
            if (entityToDelete != null)
            {
                this._context.Tasks.Remove(entityToDelete);
                await this._context.SaveChangesAsync();
            }
            return entityToDelete;
        }

        public async Task<TaskEntity> Update(TaskEntity task)
        {
            var entityToUpdate = await this._context.Tasks.FindAsync(task.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.Id = task.Id;
                entityToUpdate.Name = task.Name;
                entityToUpdate.Estimate = task.Estimate;
                entityToUpdate.Description = task.Description;
                entityToUpdate.StoryId = task.StoryId;
                entityToUpdate.Created = task.Created;
                await this._context.SaveChangesAsync();
            }
            return entityToUpdate;
        }
    }
}
