using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Todo.Data.Entities
{
        public enum TaskStatus
        {
            Undone = 0,
            InProgress = 1,
            Completed = 3,
            Trashed = 4,
        }
        public class TaskEntity
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            public TaskStatus TaskStatus { get; set; }

            [Required]
            [MaxLength(200)]
            public string Name { get; set; }

            [MaxLength(1024)]
            public string Description { get; set; }

            public int Estimate { get; set; }

            public StoryEntity Story { get; set; }

            [ForeignKey("StoryId")]
            public int StoryId { get; set; }

            public DateTimeOffset Created { get; set; }
    }
}
