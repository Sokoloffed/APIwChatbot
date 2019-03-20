using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess
{
    class DataRepository
    {
        public IQueryable<Users> GetAllUsers()
        {
            using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
            {
                return ctx.Users;
            }
        }

        public IQueryable<Users> GetUserViaID(int ID)
        {
            using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
            {
                return ctx.Users.Where(i => i.id == ID).Select(e => e);
            }

        }

        public IQueryable<Users> GetUserViaName(string name)
        {
            using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
            {
                return ctx.Users.Where(i => i.username.Equals(name)).Select(e => e);
            }
        }

        public IQueryable<Users> GetUserViaPassword(string pass)
        {
            using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
            {
                return ctx.Users.Where(i => i.password.Equals(pass)).Select(e => e);
            }
        }

        /*public IQueryable<Users> GetUserViaTask(int t_id)
        {
            using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
            {
                return ctx.Users.Where(a => a.id)
                return ctx.Users.Where(i => i.password.Equals(pass)).Select(e => e);
            }
        }*/
    }
}
