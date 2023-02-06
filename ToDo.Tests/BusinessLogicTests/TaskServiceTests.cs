using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Core.Repositories;
using Todo.Core.Services;
using DeepEqual.Syntax;
using Todo.Data.Entities;
using Xunit;

namespace ToDo.Tests.BusinessLogicTests
{
    public class TaskServiceTests
    {
        private readonly IServiceProvider serviceProvider;

        public TaskServiceTests()
        {
            var services = new ServiceCollection();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaskEntity, TaskDto>();
            });
            services.AddSingleton(config.CreateMapper());
            services.AddTransient(sp => Mock.Of<ILogger<TaskService>>());

            services.AddSingleton(sp => Mock.Of<ITaskRepository>());
            services.AddTransient<ITaskService, TaskService>();
            this.serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetTaskEntities_ShouldReturnDtos()
        {
            {
                var tasksDb = Mock.Get(this.serviceProvider.GetService<ITaskRepository>());

                tasksDb.Setup(x => x.GetAll())
                    .ReturnsAsync(new List<TaskEntity>
                    {
                    new TaskEntity { Id = 1, Description = "This is Description 1",  },
                    new TaskEntity { Id = 2, Description = "This is Description 2",  },
                    }
                );

                var sut = this.serviceProvider.GetService<ITaskService>();
                var actual = await sut.GetAllTasks();

                var expected = new List<TaskDto>
                {
                    new TaskDto { Id = 1, Description = "This is Description 1",  },
                    new TaskDto { Id = 2, Description = "This is Description 2",  },
                };


                actual.ShouldDeepEqual(expected);
            };
        }
    }
}
