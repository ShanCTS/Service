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
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("get/{id}")]
        public List<UserDto> Users(int id)
        {
            var users = new List<UserDto>();

            using (var context = new FSEEntities())
            {
                var userList = context.Users.Where(x => id == 0 || x.Id == id ).ToList();

                if(userList != null && userList.Any())
                {
                    userList.ForEach(x => 
                    {
                        users.Add(new UserDto
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            EmployeeId = x.EmployeeId
                        });
                    });
                }
            }

            return users;
        }

        [HttpGet]
        [Route("search/{criteria}")]
        public List<UserDto> searchData(string criteria)
        {
            var data = new List<UserDto>();
            
            //if(string.IsNullOrWhiteSpace(criteria))
            //{
            //    return Users(0);
            //}

            using (var context = new FSEEntities())
            {
                var list = context.Users.Where(x => criteria == "0" || x.FirstName.Contains(criteria) || x.LastName.Contains(criteria)).ToList();

                if (list != null && list.Any())
                {
                    list.ForEach(x =>
                    {
                        data.Add(new UserDto
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            EmployeeId = x.EmployeeId
                        });
                    });
                }
            }

            return data;
        }

        [HttpPost]
        [Route("save")]
        public bool SaveUser(UserDto userdata)
        {
            if (userdata != null)
            {
                if (userdata.Id == 0)
                {
                    try
                    {
                        using (var context = new FSEEntities())
                        {
                            var newUser = new User()
                            {
                                FirstName = userdata.FirstName,
                                LastName = userdata.LastName,
                                EmployeeId = userdata.EmployeeId
                            };
                            context.Users.Add(newUser);

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
                            var userDetails = context.Users.FirstOrDefault(x => x.Id == userdata.Id);

                            if (userDetails != null)
                            {
                                userDetails.FirstName = userdata.FirstName;
                                userDetails.LastName = userdata.LastName;
                                userDetails.EmployeeId = userdata.EmployeeId;
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
            return true;
        }

        [HttpPost]
        [Route("delete")]
        public bool deleteUser([FromBody]int id)
        {
            try
            {
                using (var context = new FSEEntities())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);

                    if (user != null)
                    {
                        context.Users.Remove(user);
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
