using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Data.Entities;

namespace Todo.Core.Repositories
{
    public interface IStoryRepository
    {
        Task<IEnumerable<StoryEntity>> GetAll();
        Task<StoryEntity> GetStory(int id);
        Task<StoryEntity> Create(StoryEntity story);

        Task<StoryEntity> Delete(int id);

        Task<StoryEntity> Update(StoryEntity story);

    }
}
