using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RESTApi.Models;
using DBAccess;

namespace RESTApi.Controllers
{
    public class BranchesController : ApiController
    {
        ModelFactory modelFactory;

        public BranchesController()
        {
            this.modelFactory = new ModelFactory();
        }

        [HttpGet]
        public IEnumerable<BranchesModel> Get()
        {
            DataRepository repository = new DataRepository();
            return repository.GetAllBranches().ToList().Select(c => modelFactory.Create(c));
        }

        [Route("api/Branches/GetId")]
        [HttpGet]
        public BranchesModel GetId(int id)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return modelFactory.Create(entities.Branches.Where(i => i.id == id).FirstOrDefault());
            }
        }

        [Route("api/Branches/GetName")]
        [HttpGet]
        public BranchesModel GetName(string name)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return modelFactory.Create(entities.Branches.Where(i => i.branchname.Equals(name)).FirstOrDefault());
            }
        }

        [Route("api/Branches/GetCreator")]
        [HttpGet]
        public IEnumerable<BranchesModel> GetCreator(int c_id)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Branches.Where(i => i.creator_id == c_id).ToList().Select(c => modelFactory.Create(c));
            }
        }

        [Route("api/Branches/GetBegin")]
        [HttpGet]
        public IEnumerable<BranchesModel> GetBegin(DateTime begin)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                return entities.Branches.Where(i => i.created_date.Equals(begin)).ToList().Select(c => modelFactory.Create(c));
            }
        }

        [AcceptVerbs("GET","POST")]
        public bool Post(Branches branch)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                if (entities.Branches.Where(b => b.branchname.Equals(branch.branchname)).FirstOrDefault() != null)
                {
                    return false;
                }
                entities.Branches.Add(new Branches { branchname = branch.branchname, description = branch.description, creator_id = branch.creator_id, created_date = branch.created_date });
                entities.SaveChanges();
                return true;
            }
        }

        [HttpDelete]
        public bool Delete(Branches branchDel)
        {
            using (TaskManagerDBEntities entities = new TaskManagerDBEntities())
            {
                if((entities.UserBranch.Where(i => i.branch_id==branchDel.id).FirstOrDefault() != null) || (entities.TaskBranch.Where(i => i.branch_id == branchDel.id).FirstOrDefault() != null))
                {
                    return false;
                }
                Branches branch = entities.Branches.Where(i => i.branchname == branchDel.branchname).FirstOrDefault();
                if (branch != null)
                {
                    entities.Branches.Remove(branch);
                    entities.SaveChanges();
                    return true;
                }
                else return false;
            }
        }
    }
}
