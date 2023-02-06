using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Core.Models
{
     public class StoryWithoutTasksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TaskCount { get; set; }

    }
}
