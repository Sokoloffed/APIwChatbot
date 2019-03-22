using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data.Entity;
using RESTApi.Models;


namespace RESTApi.Controllers
{
    public class UsersController : ApiController
    {
        ModelFactory modelFactory;

        public UsersController()
        {
            this.modelFactory = new ModelFactory();
        }


        [System.Web.Http.HttpGet]
        public IEnumerable<UsersModel> Get()
        {
            DataRepository repository = new DataRepository();
            return repository.GetAllUsers().ToList().Select(c => modelFactory.Create(c));
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<UsersModel> Get(int ID)
        {
            DataRepository repository = new DataRepository();
            return repository.GetUserViaID(ID).ToList().Select(c => modelFactory.Create(c));
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<UsersModel> Get(string name)
        {
            DataRepository repository = new DataRepository();
            return repository.GetUserViaName(name).ToList().Select(c => modelFactory.Create(c));
        }

        
        [Route("api/Users/CreateUser")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public string CreateUser(Users user)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {

                if (entities.Users.Where(t => (t.id == user.id)).FirstOrDefault() != null)
                {
                    return "Attempt to add the existing user.";
                }
                entities.Users.Add(user);
                entities.SaveChanges();
                return "Success!";
            }
        }
    }

}
