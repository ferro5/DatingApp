using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.DTO;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        Task<User> GetById(int id);
        Task<User> Create(User user,string password);
        void Update(User user, string password=null);
        void Delete(int id);

    }
}
