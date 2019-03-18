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
        public IEnumerable<Users> Get()
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Users.ToList();
            }
        }

        public Users Get(int ID)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                Users user = new Users();
                user.id = ID;
                user.username = "vasya";
                user.password = "1111";
                return user;
                //return entities.Users.FirstOrDefault(i => i.id == ID);
            }
        }

        public Users Get(string name)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Users.FirstOrDefault(i => i.username.Equals(name));
            }
        }

        [HttpGet]
        public Users GetViaTask(int id)
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
    }
}
