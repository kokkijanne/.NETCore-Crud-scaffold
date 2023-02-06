using Microsoft.EntityFrameworkCore;
using System;
using Todo.Data.Entities;

namespace Todo.Data
{
    public class ToDoContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<StoryEntity> Stories { get; set; }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        /// Parameterless constructor for test use
        public ToDoContext()
        {
        }

       

    }
}
