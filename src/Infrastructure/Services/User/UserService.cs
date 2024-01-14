using Domain.Entities;
using Infrastructure.Interfaces.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RamsonDevelopers.UtilEmail;
using Shared;
using Shared.Config;
using Shared.Modals;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Serilog;
using Shared.Enums.Users;
using Shared.Requests.Users;
using Shared.Responses.Users;

namespace Infrastructure.Services.User
{
    public sealed class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser<Guid>> _userManager;
        private readonly IEmailService _emailService;
        private readonly AppConfiguration _appConfig;
        private readonly ILogger _logger;

        public UserService(
            UserManager<ApplicationUser<Guid>> userManager,
            IEmailService emailService,
            IOptions<AppConfiguration> options,
            ILogger logger)
        {
            _userManager = userManager;
            _emailService = emailService;
            _appConfig = options.Value;
            _logger = logger;
        }


        /// <summary>
        /// Register User in the application and send verification email if provided.
        /// </summary>
        /// <param name="userDetails">The basic details of the user whose account is been added</param>
        /// <param name="requireEmailVerification">If the user requires to verify their account before login in.</param>
        /// <returns>Newly created user and the registration stage where the user is right now.</returns>
        public async Task<ApiResponseModal<RegisterResponse>> RegisterUser(RegisterUserRequest userDetails,
            bool requireEmailVerification = false)
        {
            try
            {
                // TODO: Validate request object
                // Check for existing users
                var existingUser = await _userManager.FindByEmailAsync(userDetails.Email);
                if (existingUser != null)
                {
                    // Return if is an existing user
                    return await ApiResponseModal<RegisterResponse>.FatalAsync(
                        new ApiErrorException(BaseErrorCodes.EmailTaken), BaseErrorCodes.EmailTaken, _logger);
                }

                // create user's record
                var applicationUser = new ApplicationUser<Guid>
                {
                    UserName = userDetails.UserName,
                    NormalizedUserName = userDetails.UserName.ToUpper(),
                    Email = userDetails.Email,
                    NormalizedEmail = userDetails.UserName.ToUpper(),
                    EmailConfirmed = !requireEmailVerification,
                    PhoneNumber = userDetails.MobileNumber,
                    PhoneNumberConfirmed = !requireEmailVerification,
                    // Check if requireEmailVerification is true
                    RegistrationStage = requireEmailVerification
                        ? RegistrationStages
                            .Registered // set registration stage to 'Registered' if requireEmailVerification is true
                        : RegistrationStages
                            .Completed // set registration stage to 'Completed' if requireEmailVerification is false
                };


                var userResponse = await _userManager.CreateAsync(applicationUser, userDetails.Password);

                if (userResponse is not { Succeeded: true })
                {
                    throw new ApiErrorException(BaseErrorCodes.IdentityError);
                }

                // if true then send confirm email
                await SendRegistrationEmail(applicationUser.UserName, applicationUser.Email);
                // Return newly created user back
                var response = new RegisterResponse
                {
                    Email = userDetails.Email,
                    RegistrationStage = applicationUser.RegistrationStage,
                };

                return await ApiResponseModal<RegisterResponse>.SuccessAsync(response);
            }
            catch (ApiErrorException e)
            {
                return await ApiResponseModal<RegisterResponse>.FatalAsync(e, e.ErrorCode, _logger);
            }
        }

        /// <summary>
        /// Generate Authentication Token and log user in.
        /// </summary>
        /// <param name="loginUserRequest">Login Credentials</param>
        /// <param name="requireEmailVerification">Does the user require Email verification to be done to log in.</param>
        /// <returns></returns>
        public async Task<ApiResponseModal<LoginResponse>> UserLogin(LoginUserRequest loginUserRequest,
            bool requireEmailVerification = false)
        {
            try
            {
                // Check if user is an existing member of the system.
                var user = await _userManager.FindByNameAsync(loginUserRequest.UserName);
                // Throw exception of UserNotFound if user is null.
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(loginUserRequest.UserName);

                    if (user is null)
                        throw new ApiErrorException(BaseErrorCodes.UserNotFound);
                }

                if (requireEmailVerification && !user.EmailConfirmed)
                {
                    throw new ApiErrorException(BaseErrorCodes.EmailNotVerified);
                }

                // Check if the user's registration stage is complete
                if (user.RegistrationStage != RegistrationStages.Completed)
                {
                    throw user.RegistrationStage switch
                    {
                        RegistrationStages.EmailVerification => new ApiErrorException(BaseErrorCodes.EmailNotVerified),
                        RegistrationStages.MobileVerification =>
                            new ApiErrorException(BaseErrorCodes.MobileNotVerified),
                        RegistrationStages.None => new ApiErrorException(BaseErrorCodes.MobileNotVerified),
                        _ => throw new ApiErrorException(BaseErrorCodes.ArgumentNull)
                    };
                }

                // Check credentials provided by user
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginUserRequest.Password);
                if (!passwordCheck) throw new ApiErrorException(BaseErrorCodes.IncorrectCredentials);

                // Create User's Token required to authenticate
                var token = await GenerateJwtTokenAsync(user);

                // Return Login Response
                var response = new LoginResponse
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Token = token
                };

                return await ApiResponseModal<LoginResponse>.SuccessAsync(response);
            }
            catch (ApiErrorException e)
            {
                return await ApiResponseModal<LoginResponse>.FatalAsync(e, _logger);
            }
            catch (Exception e)
            {
                return await ApiResponseModal<LoginResponse>.FatalAsync(e, BaseErrorCodes.UnknownSystemException,
                    _logger);
            }
        }

        public Task<ApiResponseModal> ChangePassword(object userDetails)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponseModal> ForgetPassword(object userDetails)
        {
            throw new NotImplementedException();
        }

        //public virtual Task ChangeEmail(object userDetails)
        //{
        //    throw new NotImplementedException();
        //}
        //public virtual Task ChangePhone(object userDetails)
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Abstract method to Send Emails after registration to verify
        /// </summary>
        /// <returns>Task</returns>
        public async Task SendRegistrationEmail(string name, string emailAddress)
        {
            var request = new SendEmailRequest
            {
                ToAddresses = new List<EmailAddress>
                {
                    new(name, emailAddress)
                },
                Subject = "Registration Successful",
                Body =
                    @$"You have successfully registered to {_appConfig.ApplicationName}. 
                        Please verify your email address to move forward"
            };

            await _emailService.SendEMailAsync(request);
        }

        /// <summary>
        /// Abstract Method to Generate Jwt Token based on roles and claims assigned to user
        /// </summary>
        /// <param name="user">The User whose token should be generated.</param>
        /// <returns>the Jwt Token</returns>
        public Task<string> GenerateJwtTokenAsync(ApplicationUser<Guid> user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.JwtSecurityKey)), "HS256");

            var jwtIssuer = _appConfig.JwtIssuer;
            var jwtAudience = _appConfig.JwtAudience;
            var expires = DateTime.UtcNow.AddHours(_appConfig.JwtExpiry);
            var notBefore = new DateTime?();

            var encryptedToken = new JwtSecurityTokenHandler()
                .WriteToken(new JwtSecurityToken(jwtIssuer, jwtAudience, new List<Claim>
                    {
                        new("UserId", user.Id.ToString()),
                        new("UserEmail", user.Email!),
                        new("UserName", user.UserName),
                    },
                    notBefore, expires, signingCredentials));

            return Task.FromResult(encryptedToken);
        }


        public Task MobileVerification(ApplicationUser<Guid> userDetails)
            => throw new NotImplementedException();


        public Task EmailVerification(ApplicationUser<Guid> userDetails)
            => throw new NotImplementedException();
    }
}