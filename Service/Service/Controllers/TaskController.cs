using EFModel;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Service.Controllers
{
    [RoutePrefix("api/tasks")]
    public class TaskController : ApiController
    {
        [HttpGet]
        [Route("get/{id}")]
        public List<TaskDto> Tasks(int id)
        {
            var tasks = new List<TaskDto>();

            using (var context = new FSEEntities())
            {
                var list = context.Tasks.Where(x => id == 0 || x.Id == id).ToList();

                if (list != null && list.Any())
                {
                    list.ForEach(x =>
                    {
                        var user = x.Users?.FirstOrDefault();
                        var taskUser = new UserDto();
                        if(user != null)
                        {
                            taskUser = new UserDto {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                EmployeeId = user.EmployeeId
                            };
                        }

                        tasks.Add(new TaskDto
                        {
                            Id = x.Id,
                            Description = x.Description,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            Priority = x.Priority,
                            Status = x.Status,
                            User = taskUser,
                            Project = new ProjectDto
                            {
                                Id = x.Project?.Id ?? 0,
                                Name = x.Project?.Name,
                            },
                            ParentId = x.ParentId,
                            ParentTaskName = x.ParentTask?.Description
                    });
                    });
                }
            }

            return tasks;
        }

        [HttpGet]
        [Route("search/{criteria}")]
        public List<TaskDto> searchData(string criteria)
        {
            var tasks = new List<TaskDto>();

            using (var context = new FSEEntities())
            {
                var list = context.Tasks.Where(x => criteria == null || x.Description.Contains(criteria)).ToList();

                if (list != null && list.Any())
                {
                    list.ForEach(x =>
                    {
                        var user = x.Users?.FirstOrDefault();
                        var taskUser = new UserDto();
                        if (user != null)
                        {
                            taskUser = new UserDto
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                EmployeeId = user.EmployeeId
                            };
                        }

                        tasks.Add(new TaskDto
                        {
                            Id = x.Id,
                            Description = x.Description,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            Priority = x.Priority,
                            Status = x.Status,
                            User = taskUser,
                            Project = new ProjectDto
                            {
                                Id = x.Project?.Id ?? 0,
                                Name = x.Project?.Name,
                            },
                            ParentId = x.ParentId,
                            ParentTaskName = x.ParentTask?.Description
                        });
                    });
                }
            }

            return tasks;
        }

        [HttpGet]
        [Route("parent-task/get/{id}")]
        public List<ParentTaskDto> GetParentTask(int id)
        {
            var tasks = new List<ParentTaskDto>();

            using (var context = new FSEEntities())
            {
                var list = context.ParentTasks.Where(x => id == 0 || x.Id == id).ToList();

                if (list != null && list.Any())
                {
                    list.ForEach(x =>
                    {
                        tasks.Add(new ParentTaskDto
                        {
                            Id = x.Id,
                            Description = x.Description                            
                        });
                    });
                }
            }

            return tasks;
        }

        [HttpPost]
        [Route("save")]
        public bool SaveTasks(TaskDto data)
        {
            if (data != null)
            {
                if (data.IsParentTask)
                {
                    using (var context = new FSEEntities())
                    {
                        var newParentTask = new ParentTask()
                        {
                            Description = data.Description,                            
                        };
                        context.ParentTasks.Add(newParentTask);

                        context.SaveChanges();
                    }
                }
                else
                {
                    if (data.Id == 0)
                    {
                        try
                        {
                            using (var context = new FSEEntities())
                            {
                                var newTask = new Task()
                                {
                                    Description = data.Description,
                                    StartDate = data.StartDate,
                                    EndDate = data.EndDate,
                                    Priority = data.Priority,
                                    ProjectId = data.ProjectId,
                                    ParentId = data.ParentId,
                                    Status = "New"
                                };
                                context.Tasks.Add(newTask);

                                var user = context.Users.FirstOrDefault(x => x.Id == data.UserId);
                                user.TaskId = newTask.Id;

                                context.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            using (var context = new FSEEntities())
                            {
                                var task = context.Tasks.FirstOrDefault(x => x.Id == data.Id);

                                if (task != null)
                                {
                                    task.Description = data.Description;
                                    task.StartDate = data.StartDate;
                                    task.EndDate = data.EndDate;
                                    task.Priority = data.Priority;
                                    task.ProjectId = data.ProjectId;
                                    task.ParentId = data.ParentId;
                                    task.Status = "InProgress";

                                    var user = context.Users.FirstOrDefault(x => x.Id == data.UserId);
                                    user.TaskId = task.Id;
                                }
                                else
                                {
                                    return false;
                                }

                                context.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        [HttpPost]
        [Route("delete")]
        public bool delete(int id)
        {
            try
            {
                using (var context = new FSEEntities())
                {
                    var data = context.Tasks.FirstOrDefault(x => x.Id == id);
                    data.Status = "Completed";

                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }
    }
}
