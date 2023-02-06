using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Core.Models
{
    public enum TaskStatus
    {
        Undone = 0,
        InProgress = 1,
        Completed = 3,
        Trashed = 4,
    }
    public class TaskDto
    {
        public int Id { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public string Name { get; set; }
        public int Estimate { get; set; }
        public string Description { get; set; }
        public int StoryId { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
