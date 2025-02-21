using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessEntities;

namespace WebApi.Models.Users
{
    public class UserModel
    {
        // I am adding required data annotation for validation
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public UserTypes Type { get; set; }

        // I am adding the property Age in case should it be used 
        public int Age { get; set; }

        public decimal? AnnualSalary { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}