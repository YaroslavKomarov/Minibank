using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.DTO;

namespace Minibank.Web.Controllers.Users
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService; 

        public UserController(IUserService service)
        {
            userService = service;
        }

        [HttpPost]
        public void PostUser(PostUserDto model)
        {
            userService.PostUser(new User
            {
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete]
        public void DeleteUserById(string id)
        {
            userService.DeleteUserById(id);
        }

        [HttpPut]
        public void PutUser(UserDto model)
        {
            userService.PutUser(new User
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            });
        }
    }
}
