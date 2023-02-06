
using DeepEqual.Syntax;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Repositories;
using Todo.Data;
using Todo.Data.Entities;
using Xunit;

namespace ToDo.Tests.DataLayerTests
{
    public class StoryRepositoryTests
    {
        private readonly IServiceProvider serviceProvider;

        public StoryRepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton(sp => Mock.Of<IStoryRepository>());
            services.AddTransient<IStoryRepository, StoryRepository>();
            services.AddEntityFrameworkSqlite()
            .AddDbContext<ToDoContext>(options => options.UseSqlite(new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = ":memory:" }.ConnectionString)));
            this.serviceProvider = services.BuildServiceProvider();

            // Initialize db
            var db = this.serviceProvider.GetService<ToDoContext>();
            db.Database.OpenConnection();
            db.Database.EnsureCreated();
        }

        [Fact]
        public async Task AddStoryShouldReturnStoryWithId()
        {
            var mockEntity1 = new StoryEntity
            {
                Name ="name for entity1",
                Description = "comment for entity1",
            };
            var sut = this.serviceProvider.GetService<IStoryRepository>();
            var created = await sut.Create(mockEntity1);

            Assert.True(created.Id > 0);
            Assert.Equal(created.Name, mockEntity1.Name);
            Assert.Equal(created.Description, mockEntity1.Description);
        }

        [Fact]
        public async Task CreateStories_ShouldGoIntoDb()
        {
            var createdMockEntity = new StoryEntity
            {
                Name = "name for entity1",
                Description = "This was created",
            };

            var repo = this.serviceProvider.GetService<ToDoContext>();
            repo.Stories.Add(createdMockEntity);
            await repo.SaveChangesAsync();

            var sut = this.serviceProvider.GetService<IStoryRepository>();
            var Stories = await sut.GetAll();
            var incrementedId = Stories.Count();
            var expected = new List<StoryEntity>
            {
                new StoryEntity {
                    Id = incrementedId,
                    Name = "name for entity1",
                    Description = "This was created",
                }
            };
            
            Stories.Where(x=> x.Id == incrementedId).ShouldDeepEqual(expected);
        }

        [Fact]
        public async Task DeleteStories_ShouldRemoveFromDb()
        {
            var entityToNotBeDeleted = new StoryEntity
            {
                Name = "name for entity1",
                Description = "This is supposed to stay",
            };

            var entityToBeDeleted = new StoryEntity
            {
                Name = "name for entity2",
                Description = "This is supposed to be removed",
            };

            var repo = this.serviceProvider.GetService<ToDoContext>();
            repo.Stories.Add(entityToNotBeDeleted);
            repo.Stories.Add(entityToBeDeleted);
            await repo.SaveChangesAsync();

            var sut = this.serviceProvider.GetService<IStoryRepository>();
            await sut.Delete(2);
            var Stories = await sut.GetAll();
            var expected = new List<StoryEntity> { new StoryEntity {
                Id = 1,
                Name = "name for entity1",
                Description = "This is supposed to stay",
            }
            };

            Stories.ShouldDeepEqual(expected);
        }

        [Fact]
        public async Task UpdateStories_ShouldUpdateEntry()
        {
            var createdMockEntity = new StoryEntity
            {
                Id = 1,
                Name = "name for entity1",
                Description = "This entity is to be updated",
            };

            var updatedMockEntity = new StoryEntity
            {
                Id = 1,
                Name = "this was updated",
                Description = "This entity was succesfully updated",
            };

            var repo = this.serviceProvider.GetService<ToDoContext>();
            repo.Stories.Add(createdMockEntity);
            await repo.SaveChangesAsync();

            var sut = this.serviceProvider.GetService<IStoryRepository>();
            await sut.Update(updatedMockEntity);
            await repo.SaveChangesAsync();

            var Stories = await sut.GetAll();

            var expected = new List<StoryEntity>
            {
                new StoryEntity {

                    Id = 1,
                    Name = "this was updated",
                    Description = "This entity was succesfully updated",
                }
            };

            Stories.ShouldDeepEqual(expected);

        }
    }
}

