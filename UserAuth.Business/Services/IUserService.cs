using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Data.Models;

namespace UserAuth.Business.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(int id);
        void RegisterUser(User user);
        bool VerifyUser(string email, string password);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
