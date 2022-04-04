using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.DTO;
using System.Threading.Tasks;

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
        public async Task<string> CreateUserAsync(CreateUserDto model)
        {
            return await userService.CreateUserAsync(new User
            {
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete]
        public async Task DeleteUserByIdAsync(string id)
        {
            await userService.DeleteUserByIdAsync(id);
        }

        [HttpPut]
        public async Task UpdateUserAsync([FromBody] UserDto model)
        {
            await userService.UpdateUserAsync(new User
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            });
        }
    }
}
