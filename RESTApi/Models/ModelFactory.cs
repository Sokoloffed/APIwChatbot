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
    }
}