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
                return entities.Users.FirstOrDefault(i => i.id == ID);
            }
        }

        public Users Get(string name)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Users.FirstOrDefault(i => i.username.Equals(name));
            }
        }
    }
}
