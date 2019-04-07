using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Models
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public System.DateTime? EndDate { get; set; }
        public int? Priority { get; set; }
        public string Status { get; set; }
        public bool IsParentTask { get; set; }

        public int UserId { get; set; }
        public UserDto User { get; set; }
        public ProjectDto Project { get; set; }

        public string ParentTaskName { get; set; }
    }
}