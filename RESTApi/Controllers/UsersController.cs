﻿using System;
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

        [Route("api/Users/GetList/{name}")]
        [HttpGet]
        public List<Users> GetList(string name)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                if (name == "all") return entities.Users.ToList();
                else return entities.Users.Where(i => i.username.Equals(name)).ToList();
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
        

        [Route("api/Users/CreateUser/{ID}")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public string CreateUser(int ID)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                try
                {
                    Users user = new Users();
                    user.id = ID;
                    user.username = "testNewPost";
                    user.password = "testNewPostPassword";
                    entities.Users.Add(user);
                    entities.SaveChanges();
                    return "Entered successfully!";

                }
                catch
                {
                    return "Exception occured!";
                }
                
            }
        }
    }

}
