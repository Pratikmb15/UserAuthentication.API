using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Data.Models;
using UserAuth.Data.Repositories;

namespace UserAuth.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers() => _userRepository.GetUsers();

        public User GetUser(int id) => _userRepository.GetUserById(id);

        public void RegisterUser(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _userRepository.AddUser(user);
        }

        public bool VerifyUser(string email, string password)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(u => u.Email == email);
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public void UpdateUser(User user) => _userRepository.UpdateUser(user);

        public void DeleteUser(int id) => _userRepository.DeleteUser(id);
    }
}
