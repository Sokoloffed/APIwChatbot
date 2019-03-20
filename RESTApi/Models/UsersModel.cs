using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTApi.Models
{
    public class UsersModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public IEnumerable<TasksModel> Tasks { get; set; }
        public IEnumerable<BranchesModel> Branches { get; set; }
    }
}