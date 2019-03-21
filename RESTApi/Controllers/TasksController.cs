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

        public IEnumerable<TasksModel> Get(int id)
        {
            DataRepository repository = new DataRepository();
            return repository.GetTaskViaID(id).ToList().Select(c => modelFactory.Create(c));
        }
    }
}
