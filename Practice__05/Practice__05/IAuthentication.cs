using Practice__05;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice__05
{
    public interface IAuthentication
    {
        AuthenticationCode IsAuthenticatedUser(string id, string password);
        bool ModifyUser(string id, IUserView user);
        bool InsertUser(IUserView user);
        bool DeleteUser(string id);
        IUserView GetUser(string id);
        void SaveData();

    }
}
