
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
    public class TaskRepositoryTests
    {
        private readonly IServiceProvider serviceProvider;

        public TaskRepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton(sp => Mock.Of<ITaskRepository>());
            services.AddSingleton(sp => Mock.Of<IStoryRepository>());
            services.AddTransient<ITaskRepository, TaskRepository>();
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
        public async Task AddTaskhouldReturnTaskithId()
        {
            var mockStoryForFKConstraint = new StoryEntity
            {
                Id = 1,
                Name = "name for MockStory",
            };

            var repo = this.serviceProvider.GetService<ToDoContext>();
            repo.Stories.Add(mockStoryForFKConstraint);
            await repo.SaveChangesAsync();

            var mockEntity1 = new TaskEntity
            {
                StoryId = 1,
                Name = "name for entity1",
                Description = "comment for entity1",
            };

            var sut = this.serviceProvider.GetService<ITaskRepository>();
            var created = await sut.Create(mockEntity1);

            Assert.True(created.Id == 1);
            Assert.Equal(created.Name, mockEntity1.Name);
            Assert.Equal(created.Description, mockEntity1.Description);
            Assert.Equal(created.StoryId, mockEntity1.StoryId);
        }

        [Fact]
        public async Task DeleteTasks_ShouldRemoveFromDb()
        {
            var mockStoryForFKConstraint = new StoryEntity
            {
                Id = 1,
                Name = "name for MockStory",
            };

            var repo = this.serviceProvider.GetService<ToDoContext>();
            repo.Stories.Add(mockStoryForFKConstraint);
            await repo.SaveChangesAsync();


            var entityToNotBeDeleted = new TaskEntity
            {
                StoryId = 1,
                Name = "name for entity1",
                Description = "This is supposed to stay",
            };

            var entityToBeDeleted = new TaskEntity
            {
                StoryId = 1,
                Name = "name for entity2",
                Description = "This is supposed to be removed",
            };

            repo.Tasks.Add(entityToNotBeDeleted);
            repo.Tasks.Add(entityToBeDeleted);
            await repo.SaveChangesAsync();

            var sut = this.serviceProvider.GetService<ITaskRepository>();
            await sut.Delete(2);
            var Tasks = await sut.GetAll();
            var expected = new List<TaskEntity> { new TaskEntity {
                Id = 1,
                Name = "name for entity1",
                Description = "This is supposed to stay",
                StoryId = 1,
                Story = mockStoryForFKConstraint,
            }
            };

            Tasks.ShouldDeepEqual(expected);
        }

        [Fact]
        public async Task UpdateTasks_ShouldUpdateEntry()
        {
            var mockStoryForFKConstraint = new StoryEntity
            {
                Id = 1,
                Name = "name for MockStory",
            };

            var repo = this.serviceProvider.GetService<ToDoContext>();
            repo.Stories.Add(mockStoryForFKConstraint);
            await repo.SaveChangesAsync();

            var originalTaskEntity = new TaskEntity
            {
                Id = 1,
                StoryId = 1,
                Name = "original name",
                Description = "original description",
            };

            repo.Tasks.Add(originalTaskEntity);

            var updatedMockEntity = new TaskEntity
            {
                Id = 1,
                StoryId = 1,
                Name = "updated name",
                Description = "updated description",
            };
            var sut = this.serviceProvider.GetService<ITaskRepository>();
            await sut.Update(updatedMockEntity);
            await repo.SaveChangesAsync();

            var Tasks = await sut.GetAll();

            var expected = new List<TaskEntity>
            {
                new TaskEntity {
                    Id = 1,
                    StoryId = 1,
                    Name = "updated name",
                    Description = "updated description",
                    Story = mockStoryForFKConstraint,

                }
            };

            Tasks.ShouldDeepEqual(expected);


        }


    }
}

