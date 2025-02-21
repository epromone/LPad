using System.Collections.Generic;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        // I am icluding parameter age in case it is being part of the request 
        public void Update(User user, string name, string email, UserTypes type, int age, decimal? annualSalary, IEnumerable<string> tags)
        {
            user.SetEmail(email);
            user.SetName(name);
            user.SetType(type);

            // I am setting the age here
            user.SetAge(age);          

            // The calculation annualSalary.Value / 12 should be used if and only if annualSalary has a value. Othewise it will throw an exception.
            // I am using the ternary func to fix the issue.
            user.SetMonthlySalary(annualSalary.HasValue ? annualSalary.Value / 12 : 0);
            user.SetTags(tags);
        }
    }
}