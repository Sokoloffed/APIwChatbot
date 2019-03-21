using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBAccess;

namespace RESTApi.Models
{
    public class ModelFactory
    {
        public UsersModel Create(Users user)
        {
            return new UsersModel()
            {
                id = user.id,
                username = user.username,
                password = user.password
            };
        } 

        public TasksModel Create(Tasks task)
        {
            return new TasksModel()
            {
                id = task.id,
                taskname = task.taskname,
                description = task.description,
                date_begin = task.date_begin,
                date_end = task.date_end,
                status = task.status,
                creator_id = task.creator_id,
                executor_id = task.executor_id
            };
        }

        public BranchesModel Create(Branches branch)
        {
            return new BranchesModel()
            {
                id = branch.id,
                branchname = branch.branchname,
                description = branch.description,
                creator_id = branch.creator_id,
                created_date = branch.created_date
            };
        }
    }
}