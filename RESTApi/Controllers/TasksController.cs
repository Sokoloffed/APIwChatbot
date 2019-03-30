using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using DBAccess;
using RESTApi.Models;

namespace RESTApi.Controllers
{
    public class TasksController : ApiController
    {
        ModelFactory modelFactory;

        public TasksController()
        {
            this.modelFactory = new ModelFactory();
        }

        [HttpGet]
        public IEnumerable<TasksModel> Get()
        {
            DataRepository repository = new DataRepository();
            return repository.GetAllTasks().ToList().Select(c => modelFactory.Create(c));
        }

        [Route("api/Tasks/GetId")]
        [HttpGet]
        public TasksModel GetId(int id)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return modelFactory.Create(entities.Tasks.Where(i => i.id == id).FirstOrDefault());
            }
        }

        [Route("api/Tasks/GetBegin")]
        [HttpGet]
        public IEnumerable<TasksModel> GetBegin(DateTime begin)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Tasks.Where(i => i.date_begin == begin).ToList().Select(c => modelFactory.Create(c));
            }
        }

        [Route("api/Tasks/GetEnd")]
        [HttpGet]
        public IEnumerable<TasksModel> GetEnd(DateTime end)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Tasks.Where(i => i.date_end == end).ToList().Select(c => modelFactory.Create(c));
            }
        }

        [Route("api/Tasks/GetName")]
        [HttpGet]
        public TasksModel GetName(string name)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return modelFactory.Create(entities.Tasks.Where(i => i.taskname.Equals(name)).FirstOrDefault());
            }
        }

        [Route("api/Tasks/GetStatus")]
        [HttpGet]
        public IEnumerable<TasksModel> GetStatus(string status)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Tasks.Where(i => i.status.Equals(status)).ToList().Select(c => modelFactory.Create(c));
            }
        }

        [Route("api/Tasks/GetCreator")]
        [HttpGet]
        public IEnumerable<TasksModel> GetCreator(int c_id)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Tasks.Where(i => i.creator_id == c_id).ToList().Select(c => modelFactory.Create(c));
            }
        }

        [Route("api/Tasks/GetExecutor")]
        [HttpGet]
        public IEnumerable<TasksModel> GetExecutor(int e_id)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Tasks.Where(i => i.executor_id == e_id).ToList().Select(c => modelFactory.Create(c));
            }
        }

        [HttpPost]
        public bool Post(Tasks task)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                if(entities.Tasks.Where(t => t.taskname.Equals(task.taskname)).FirstOrDefault() != null)
                {
                    return false;
                }
                entities.Tasks.Add(task);
                entities.SaveChanges();
                return true;

            }
        }

        [HttpDelete]
        public bool Delete(Tasks taskDel)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                if((entities.TaskBranch.Where(i => i.task_id==taskDel.id).FirstOrDefault() != null) || (entities.UserTask.Where(i => i.task_id == taskDel.id).FirstOrDefault() != null))
                {
                    return false;
                }
                Tasks task = entities.Tasks.Where(i => i.taskname.Equals(taskDel.taskname)).FirstOrDefault();
                if (task != null)
                {
                    entities.Tasks.Remove(task);
                    entities.SaveChanges();
                    return true;
                }
                else return false;

            }
        }


        
    }
}
