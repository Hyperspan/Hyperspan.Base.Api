using System;
using System.Threading.Tasks;
using Hyperspan.Auth.Shared.Requests;
using Hyperspan.Auth.Shared.Responses;
using Hyperspan.Shared.Modals;

namespace Hyperspan.Auth.Interfaces
{
    public interface IUserService<T> where T : IEquatable<T>
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
