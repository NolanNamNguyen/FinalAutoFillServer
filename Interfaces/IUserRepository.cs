using FinalAutoFillServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAutoFillServer.Interfaces
{
    public interface IUserRepository
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password, string origin);
        void Update(User user, string password = null);
        void Delete(int id);
        bool CheckUser(int userId);
        void UpdateToken(User user, string currentToken);
    }
}
