using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBAccess;

namespace RESTApi.Controllers
{
    public class UsersController : ApiController
    {
        [System.Web.Http.HttpGet]
        public IEnumerable<Users> Get()
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Users.ToList();
            }
        }

        [System.Web.Http.HttpGet]
        public Users Get(int ID)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Users.FirstOrDefault(i => i.id == ID);
            }
        }

        [System.Web.Http.HttpGet]
        public Users Get(string name)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Users.FirstOrDefault(i => i.username.Equals(name));
            }
        }

        //[System.Web.Http.HttpGet]
        [HttpGet]
        public Users ViaTask(int id)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                //int u_id = entities.UserTask.FirstOrDefault(i => i.task_id == id).user_id;
                //return entities.Users.FirstOrDefault(i => i.id == u_id);
                Users user = new Users();
                user.id = id;
                user.username = $"{id}";
                user.password = "p";
                return user;

            }
        }
        
        [Route("api/Users/CreateUser/{ID}/name/password")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public string CreateUser(int ID, string name, string password)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                Users defaultUser = new Users();
                defaultUser.id = ID;
                defaultUser.username = name;
                defaultUser.password = password;
                if(entities.Users.Where(t => (t.id == ID)).FirstOrDefault() != null)
                {
                    return "Attempt to add the existing user.";
                }
                entities.Users.Add(defaultUser);
                entities.SaveChanges();
                return "Success!";
            }
        }

    }
}
