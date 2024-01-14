using Shared.Modals;
using Shared.Requests.Users;
using Shared.Responses.Users;

namespace Infrastructure.Interfaces.User
{
    public interface IUserService
    {
        Task<ApiResponseModal<RegisterResponse>> RegisterUser(RegisterUserRequest userDetails,
            bool requireEmailVerification = false);

        Task<ApiResponseModal<LoginResponse>> UserLogin(LoginUserRequest loginUserRequest,
            bool requireEmailVerification = false);

        Task<ApiResponseModal> ChangePassword(object userDetails);
        Task<ApiResponseModal> ForgetPassword(object userDetails);
        //Task ChangeEmail(object userDetails);
        //Task ChangePhone(object userDetails);

    }
}