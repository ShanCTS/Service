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
    [RoutePrefix("api/projects")]
    public class ProjectController : ApiController
    {
        [HttpGet]
        [Route("get/{id}")]
        public List<ProjectDto> Projects(int id)
        {
            var projects = new List<ProjectDto>();

            using (var context = new FSEEntities())
            {
                var list = context.Projects.Where(x => id == 0 || x.Id == id).ToList();

                if (list != null && list.Any())
                {
                    var newTasks = new List<TaskDto>();
                    list.ForEach(x =>
                    {
                        var selectedUser = x.Users?.FirstOrDefault() ?? new User();
                        var tasks = x.Tasks;
                        if(tasks != null & tasks.Any())
                        {
                            tasks.ToList().ForEach(t => 
                            {
                                newTasks.Add(new TaskDto {
                                    Id = t.Id,
                                    Description = t.Description,
                                    StartDate = t.StartDate,
                                    EndDate = t.EndDate,
                                    Priority = t.Priority,
                                    Status = t.Status,
                                    ParentId = t.ParentId,
                                    ParentTaskName = t.ParentTask?.Description
                                });
                            });
                        }
                        projects.Add(new ProjectDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            Priority = x.Priority,
                            TotalTasks = x.Tasks?.Count() ?? 0,
                            CompletedTasks = x.Tasks?.Count(z => z.Status == "Completed") ?? 0,
                            SelectedUser = new UserDto
                            {
                                Id = selectedUser.Id,
                                FirstName = selectedUser.FirstName,
                                LastName = selectedUser.LastName,
                                EmployeeId = selectedUser.EmployeeId                                
                            },
                            Tasks = newTasks
                        });
                    });
                }
            }

            return projects;
        }

        [HttpGet]
        [Route("search/{criteria}")]
        public List<ProjectDto> searchData(string criteria)
        {
            var projects = new List<ProjectDto>();

            using (var context = new FSEEntities())
            {
                var list = context.Projects.Where(x => criteria == null || x.Name.Contains(criteria)).ToList();

                if (list != null && list.Any())
                {
                    var newTasks = new List<TaskDto>();
                    list.ForEach(x =>
                    {
                        var selectedUser = x.Users?.FirstOrDefault() ?? new User();
                        var tasks = x.Tasks;
                        if (tasks != null & tasks.Any())
                        {
                            tasks.ToList().ForEach(t =>
                            {
                                newTasks.Add(new TaskDto
                                {
                                    Id = t.Id,
                                    Description = t.Description,
                                    StartDate = t.StartDate,
                                    EndDate = t.EndDate,
                                    Priority = t.Priority,
                                    Status = t.Status,
                                    ParentId = t.ParentId,
                                    ParentTaskName = t.ParentTask?.Description
                                });
                            });
                        }
                        projects.Add(new ProjectDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            Priority = x.Priority,
                            TotalTasks = x.Tasks?.Count() ?? 0,
                            CompletedTasks = x.Tasks?.Count(z => z.Status == "Completed") ?? 0,
                            SelectedUser = new UserDto
                            {
                                Id = selectedUser.Id,
                                FirstName = selectedUser.FirstName,
                                LastName = selectedUser.LastName,
                                EmployeeId = selectedUser.EmployeeId
                            },
                            Tasks = newTasks
                        });
                    });
                }
            }

            return projects;
        }

        [HttpPost]
        [Route("save")]
        public bool SaveProjects(ProjectDto data)
        {
            if (data != null)
            {
                if (data.Id == 0)
                {
                    try
                    {
                        using (var context = new FSEEntities())
                        {
                            var newProject = new Project()
                            {
                                Name = data.Name,
                                StartDate = data.StartDate,
                                EndDate = data.EndDate,
                                Priority = data.Priority
                            };
                            context.Projects.Add(newProject);

                            var user = context.Users.FirstOrDefault(x => x.Id == data.SelectedUser.Id);
                            user.ProjectId = newProject.Id;

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
                            var project = context.Projects.FirstOrDefault(x => x.Id == data.Id);

                            if (project != null)
                            {
                                project.Name = data.Name;
                                project.StartDate = data.StartDate;
                                project.EndDate = data.EndDate;
                                project.Priority = data.Priority;
                            }
                            else
                            {
                                return false;
                            }

                            var user = context.Users.FirstOrDefault(x => x.ProjectId == data.Id);
                            user.ProjectId = null;
                            var newuser = context.Users.FirstOrDefault(x => x.Id == data.SelectedUser.Id);
                            newuser.ProjectId = data.Id;

                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        [HttpPost]
        [Route("delete")]
        public bool delete([FromBody]int id)
        {
            try
            {
                using (var context = new FSEEntities())
                {
                    var data = context.Projects.FirstOrDefault(x => x.Id == id);
                    context.Projects.Remove(data);

                    var user = context.Users.FirstOrDefault(x => x.ProjectId == id);
                    if (user != null)
                    {
                        user.ProjectId = null;
                    }

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
