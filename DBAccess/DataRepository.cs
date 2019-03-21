using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess
{
    public class DataRepository
    {
        TaskManagerDBEntities ctx;
        public IQueryable<Users> GetAllUsers()
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Users;
        }

        public IQueryable<Tasks> GetAllTasks()
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks;
        }

        public IQueryable<Users> GetUserViaID(int ID)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Users.Where(i => i.id == ID);
        }

        public IQueryable<Users> GetUserViaName(string name)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Users.Where(i => i.username.Equals(name)).Select(e => e);
            
        }

        public IQueryable<Users> GetUserViaPassword(string pass)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Users.Where(i => i.password.Equals(pass)).Select(e => e);
            
        }

        public IQueryable<Tasks> GetTaskViaName(string name)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks.Where(i => i.taskname.Equals(name));
        }

        public IQueryable<Tasks> GetTaskViaID(int ID)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks.Where(i => i.id == ID);
        }

        public IQueryable<Tasks> GetTaskViaBeginTime(DateTime begin)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks.Where(i => i.date_begin == begin);
        }

        public IQueryable<Tasks> GetTaskViaID(DateTime end)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks.Where(i => i.date_end == end);
        }

        public IQueryable<Tasks> GetTaskViaStatus(string s)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks.Where(i => i.status.Equals(s));
        }

        public IQueryable<Tasks> GetTaskViaCreatorID(int ID)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks.Where(i => i.creator_id == ID);
        }

        public IQueryable<Tasks> GetTaskViaExecutorID(int ID)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Tasks.Where(i => i.executor_id == ID);
        }
        
        public IQueryable<Branches> GetAllBranches()
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Branches;
        }

        public IQueryable<Branches> GetBranchesViaID(int ID)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Branches.Where(i => i.id == ID);
        }

        public IQueryable<Branches> GetBranchesViaName(string name)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Branches.Where(i => i.branchname.Equals(name));
        }

        public IQueryable<Branches> GetBranchesViaDesc(string name)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Branches.Where(i => i.description.Equals(name));
        }

        public IQueryable<Branches> GetBranchesViaCreatorID(int ID)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Branches.Where(i => i.creator_id == ID);
        }

        public IQueryable<Branches> GetBranchesViaBeginTime(DateTime begin)
        {
            ctx = new TaskManagerDBEntities();
            return ctx.Branches.Where(i => i.created_date == begin);
        }
    }
}
