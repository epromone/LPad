using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Users
{
    public interface IUpdateUserService
    {
        // I am icluding the parameter age because in case it is being part of the request 
        void Update(User user, string name, string email, UserTypes type, int age, decimal? annualSalary, IEnumerable<string> tags);
    }
}