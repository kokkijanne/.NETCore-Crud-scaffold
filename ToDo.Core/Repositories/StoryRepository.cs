using AutoMapper.QueryableExtensions;
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
    public class StoryRepository : IStoryRepository
    {
        private readonly ToDoContext _context;

        public StoryRepository(ToDoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<StoryEntity>> GetAll()
        {
            var result = await this._context.Stories.Include(s => s.Tasks).ToListAsync();
            return result;
        }

        public async Task<StoryEntity> GetStory(int id)
        {
            var result = await this._context.Stories.Where(s => s.Id == id).Include(s=> s.Tasks ).FirstOrDefaultAsync();
            return result;

        }
        public async Task<StoryEntity> Create(StoryEntity story)
        {
            this._context.Stories.Add(story);
            await this._context.SaveChangesAsync();
            return story;
        }

      
        public async Task<StoryEntity> Delete(int storyId)
        {
            var entityToDelete = await this._context.Stories.FindAsync(storyId);
            if (entityToDelete != null)
            {
                this._context.Stories.Remove(entityToDelete);
                await this._context.SaveChangesAsync();
            }
            return entityToDelete;
        }

        public async Task<StoryEntity> Update(StoryEntity story)
        {
            var entityToUpdate = await this._context.Stories.FindAsync(story.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.Id = story.Id;
                entityToUpdate.Name = story.Name;
                entityToUpdate.Description = story.Description;
                entityToUpdate.Created = story.Created;
                await this._context.SaveChangesAsync();
            }
            return entityToUpdate;
        }

    }
}
