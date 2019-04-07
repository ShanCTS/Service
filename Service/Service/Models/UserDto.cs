using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name => FirstName + " " + LastName;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }

        public ProjectDto Project { get; set; }
        public TaskDto Task { get; set; }
    }
}