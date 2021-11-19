using BRD.DataModel.Login;
using BRD.DataModel.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Repository
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
