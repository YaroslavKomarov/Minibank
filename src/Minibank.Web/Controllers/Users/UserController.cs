using Minibank.Core.Domains.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Minibank.Web.Controllers.Users.DTO;
using Minibank.Core.Domains.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Web.Controllers.Users
{
    [ApiController]
    [Authorize]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService; 

        public UserController(IUserService service)
        {
            userService = service;
        }

        [HttpPost]
        public async Task<string> CreateUserAsync(CreateUserDto model, CancellationToken cancellationToken)
        {
            return await userService.CreateUserAsync(new User
            {
                Login = model.Login,
                Email = model.Email
            }, cancellationToken);
        }

        [HttpDelete]
        public async Task DeleteUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            await userService.DeleteUserByIdAsync(id, cancellationToken);
        }

        [HttpPut]
        public async Task UpdateUserAsync([FromBody] UserDto model, CancellationToken cancellationToken)
        {
            await userService.UpdateUserAsync(new User
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            }, cancellationToken);
        }
    }
}
