using Microsoft.AspNetCore.Mvc;
using NewApiProject.Api.Entites;

namespace NewApiProject.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUser(User user);
        Task RegisterUser(User user);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
