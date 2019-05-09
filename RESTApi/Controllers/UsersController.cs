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
        public UsersModel Get(int ID)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return modelFactory.Create(entities.Users.Where(i => i.id == ID).FirstOrDefault());
            }
        }

        [System.Web.Http.HttpGet]
        public UsersModel Get(string username, string password)
        {
            using (TaskManagerDBEntities ctx = new TaskManagerDBEntities())
            {
                return modelFactory.Create(ctx.Users.Where(i => i.username.Equals(username)).Where(i => i.password.Equals(password)).FirstOrDefault());
            }
        }

        //[HttpPut]
        //public bool Put()

        [System.Web.Http.HttpPost]
        [AcceptVerbs("GET","POST")]
        public bool Post(Users user)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                if (entities.Users.Where(t => t.username.Equals(user.username)).Where(t => t.password.Equals(user.password)).FirstOrDefault() != null)
                {
                    return false;
                }
                entities.Users.Add(new Users {username=user.username,password=user.password });
                entities.SaveChanges();
                return true;
            }
        }

        [HttpDelete]
        [AcceptVerbs("GET", "POST", "DELETE")]
        public bool Delete(int id)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                if((entities.UserBranch.Where(u=> u.user_id == id).FirstOrDefault() != null) || (entities.UserTask.Where(u => u.user_id == id).FirstOrDefault() != null)){
                    return false;
                }
                Users user = entities.Users.Where(u => u.id == id).FirstOrDefault();
                if (user != null)
                {
                    entities.Users.Remove(user);
                    entities.SaveChanges();
                    return true;
                }
                else return false;
                
            }
        }
        
        [HttpDelete]
        public bool Delete(Users userDel)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {

                if ((entities.UserBranch.Where(u => u.user_id == userDel.id).FirstOrDefault() != null) || (entities.UserTask.Where(u => u.user_id == userDel.id).FirstOrDefault() != null)){
                    return false;
                }
                if (userDel != null)
                {
                    entities.Users.Remove(userDel);
                    entities.SaveChanges();
                    return true;
                }
                else return false;
            }
        }
    }

}
