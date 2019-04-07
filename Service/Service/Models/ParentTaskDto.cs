using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Models
{
    public class ParentTaskDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
       
        public List<TaskDto> Tasks { get; set; }
    }
}