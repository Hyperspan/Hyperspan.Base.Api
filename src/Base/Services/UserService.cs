using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Auth.Services;
using Hyperspan.Base.Interfaces;
using Hyperspan.Shared.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RamsonDevelopers.UtilEmail;

namespace Hyperspan.Base.Services
{
    public class UserService : UserService<Guid>, IUserService
    {

        public UserService(
            UserManager<ApplicationUser<Guid>> userManager,
            IEmailService emailService,
            IOptions<AppConfiguration> options)
            : base(userManager, emailService, options)
        { }

        public override async Task MobileVerification(ApplicationUser<Guid> userDetails)
        {
            throw new NotImplementedException();
        }

        public override async Task EmailVerification(ApplicationUser<Guid> userDetails)
        {
            throw new NotImplementedException();
        }
    }
}
