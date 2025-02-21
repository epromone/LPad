using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Users
{
    public interface ICreateUserService
    {
        // I am icluding parameter age because in case it is being part of the request 
        User Create(Guid id, string name, string email, UserTypes type, int age,decimal? annualSalary, IEnumerable<string> tags);
    }
}