namespace Hyperspan.Auth.Shared.Enums;

public enum RegistrationStages
{
    None = 0,
    Registered = 10,
    EmailVerification = 20,
    MobileVerification = 30,
    Completed = 40,
}