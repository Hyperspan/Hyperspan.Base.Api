using Hyperspan.Auth.Shared.Requests;
using Hyperspan.Auth.Shared.Responses;
using Hyperspan.Base.Interfaces;
using Hyperspan.Shared.Modals;
using Microsoft.AspNetCore.Mvc;

namespace Base.Api.Controllers
{
    [ApiController]
    [Route("api/account/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            => _userService = userService;


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
