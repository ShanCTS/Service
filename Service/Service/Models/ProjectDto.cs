using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Models
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Priority { get; set; }

        public int TotalTasks { get; set; }

        public int CompletedTasks { get; set; }


        public List<TaskDto> Tasks { get; set; }
        
        public List<UserDto> Users { get; set; }

        public UserDto SelectedUser { get; set; }
    }
}