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

        [HttpGet]
        public IEnumerable<BranchesModel> Get(int id)
        {
            DataRepository repository = new DataRepository();
            return repository.GetBranchesViaID(id).ToList().Select(c => modelFactory.Create(c));
        }

    }
}
