using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Todo.Core.Models
{
    public class StoryDto
    {
        public StoryDto()
        {
            Tasks = new Collection<TaskDto>();
        }

        public int TasksInTask { 
            get
            {
                return Tasks.Count;
            } 
        }

        public int Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public ICollection<TaskDto> Tasks { get; set; } = new List<TaskDto>();

       
    }
}
