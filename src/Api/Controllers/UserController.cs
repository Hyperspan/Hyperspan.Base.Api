using Infrastructure.Interfaces.User;
using Microsoft.AspNetCore.Mvc;
using Shared.Modals;
using Shared.Requests.Users;
using Shared.Responses.Users;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/account/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("register")]
        public async Task<ApiResponseModal<RegisterResponse>> RegisterUserAsync(RegisterUserRequest request)
            => await _userService.RegisterUser(request);

        [HttpPost("login")]
        public async Task<ApiResponseModal<LoginResponse>> LoginUserAsync(LoginUserRequest request)
            => await _userService.UserLogin(request);

        [HttpPost("change-password")]
        public async Task<ApiResponseModal> ChangePassword(object userDetails)
            => await _userService.ChangePassword(userDetails);

        [HttpPost("forget-password")]
        public async Task<ApiResponseModal> ForgetPassword(object userDetails)
            => await _userService.ForgetPassword(userDetails);
    }
}