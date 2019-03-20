using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTApi.Models
{
    public class BranchesModel
    {
        public int id { get; set; }
        public string branchname { get; set; }
        public string description { get; set; }
        public int creator_id { get; set; }
        public DateTime created_date { get; set; }

        public IEnumerable<UsersModel> Users { get; set; }
        public IEnumerable<TasksModel> Tasks { get; set; }
    }
}