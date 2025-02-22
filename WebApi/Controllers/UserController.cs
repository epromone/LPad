using System;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Xml.Linq;
using BusinessEntities;
using Core.Services.Users;
using WebApi.Models.Users;
using static System.Collections.Specialized.BitVector32;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;

        public UserController(ICreateUserService createUserService, IDeleteUserService deleteUserService, IGetUserService getUserService, IUpdateUserService updateUserService)
        {
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
        }

        [Route("{userId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Guid userId, [FromBody] UserModel model)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                // The resource does not exist on the server and needs to be created.
                // Normally this endpoint [Route("{userId:guid}/create")], in its original version, should be used for creating new resources.
                // I am also including the parameter model.Age in case it is being part of the request body.
                user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.Age, model.AnnualSalary, model.Tags);
            }
            else
            {
                // The resource already exists on the server, but it can be updated. 
                // It is recommended to use the other end point [Route("{userId:guid}/update")] for updating existing resources.
                // However I am providing a fix here for the sake of completing the Task POST Create http://localhost:5141/users/1422740e-6426-4c46-8445-3f5274a62424/create I have been assigned to.
                // I am also including the parameter model.Age in case it is being part of the request body.
                _updateUserService.Update(user, model.Name, model.Email, model.Type, model.Age, model.AnnualSalary, model.Tags);
            }

            return Found(new UserData(user));
        }

        [Route("{userId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateUser(Guid userId, [FromBody] UserModel model)
        {
            // Check model validity
            if (!ModelState.IsValid)
            {
                return InvalidData();
            }

            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }

            // I am also including the parameter model.Age in case it is being part of the request body.
            _updateUserService.Update(user, model.Name, model.Email, model.Type, model.Age, model.AnnualSalary, model.Tags);
            return Found(new UserData(user));
        }

        [Route("{userId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }
            _deleteUserService.Delete(user);
            return Found();
        }

        [Route("{userId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            return Found(new UserData(user));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetUsers(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            var users = _getUserService.GetUsers(type, name, email)
                                       .Skip(skip).Take(take)
                                       .Select(q => new UserData(q))
                                       .ToList();
            return Found(users);
        }

        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllUsers()
        {
            _deleteUserService.DeleteAll();
            return Found();
        }

        [Route("list/tag")]
        [HttpGet]
        public HttpResponseMessage GetUsersByTag(string tag)
        {
            //throw new NotImplementedException();

            if (string.IsNullOrWhiteSpace(tag))
            {
                return InvalidData();
            }

            // Filter users who have the given tag (case-sensitive match)
            var users = _getUserService.GetUsers()
                                      .Where(u => u.Tags != null && u.Tags.Any(t => string.Equals(t, tag, StringComparison.Ordinal)))
                                      .ToList();

            if (!users.Any())
            {
                return DoesNotExist();
            }

            return Found(users);
        }
    }
}