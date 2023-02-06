using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Todo.Core.Models;
using Todo.Core.Repositories;
using Todo.Core.Services;
using DeepEqual.Syntax;
using Todo.Data.Entities;
using Xunit;
using System.Threading.Tasks;

namespace ToDo.Tests.BusinessLogicTests
{
    public class StoryServiceTests
    {
        private readonly IServiceProvider serviceProvider;

        public StoryServiceTests()
        {
            var services = new ServiceCollection();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StoryEntity, StoryDto>();
                cfg.CreateMap<StoryEntity, StoryWithoutTasksDto>();
                cfg.CreateMap<TaskEntity, TaskDto>();
            });
            services.AddSingleton(config.CreateMapper());
            services.AddTransient(sp => Mock.Of<ILogger<StoryService>>());

            services.AddSingleton(sp => Mock.Of<IStoryRepository>());
            services.AddTransient<IStoryService, StoryService>();
            this.serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetStoryEntitiesWithoutTasks_ShouldReturnDtosWithoutTasks()
        {
            {
                var StorysDb = Mock.Get(this.serviceProvider.GetService<IStoryRepository>());

                StorysDb.Setup(x => x.GetAll())
                    .ReturnsAsync(new List<StoryEntity>
                    {
                    new StoryEntity { Id = 1, Description = "This is Description 1",  },
                    new StoryEntity { Id = 2, Description = "This is Description 2",  },
                    }
                );

                var sut = this.serviceProvider.GetService<IStoryService>();
                var actual = await sut.GetAllStories();

                var expected = new List<StoryWithoutTasksDto>
                {
                    new StoryWithoutTasksDto { Id = 1, Description = "This is Description 1",  },
                    new StoryWithoutTasksDto { Id = 2, Description = "This is Description 2",  },
                };


                actual.ShouldDeepEqual(expected);
            };
        }

        [Fact]
        public async Task GetStoryEntityWithTasks_ShouldReturnDtoWithTasks()
        {
            {
                var StorysDb = Mock.Get(this.serviceProvider.GetService<IStoryRepository>());

                StorysDb.Setup(x => x.GetStory(1))
                    .ReturnsAsync(new StoryEntity
                    {
                        Id = 1,
                        Description = "This is Description 1",
                        Tasks = new List<TaskEntity> { new TaskEntity { Name = "Test Task!" }  }
                    }
                );

                var sut = this.serviceProvider.GetService<IStoryService>();

                var taskInclusion = true;
                StoryDto actual = await sut.GetStory(1, taskInclusion);

                var expected =
                    new StoryDto
                    {
                        Id = 1,
                        Description = "This is Description 1",
                        Tasks = new List<TaskDto> { new TaskDto { Name = "Test Task!" } }
                    };
                
                actual.ShouldDeepEqual(expected);
            };
        }

    }
}
