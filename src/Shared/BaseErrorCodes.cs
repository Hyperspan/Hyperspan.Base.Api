// ReSharper disable InconsistentNaming
#pragma warning disable CS1591
global using System;
global using System.Collections.Generic;
global using System.Threading.Tasks;


namespace Hyperspan.Shared
{
    public class BaseErrorCodes
    {
        public BaseErrorCodes()
        {
            ErrorMessages = _baseErrorMessages;
        }

        protected internal IDictionary<string, string> _baseErrorMessages = new Dictionary<string, string>
        {
            #region 00 System

            { UnknownSystemException, "Some Unknown Error occurred" },
            { NotImplemented, "Method not implemented" },
            { ArgumentNull, "The argument passed was null." },
            { NullValue, "The value is null" },

            #endregion

            #region 01 Auth

            { EmailTaken, "This email address is already taken" },
            { PasswordNotStrong, "Password is not strong enough. Please try another password." },
            { IncorrectCredentials, "Credentials provided are not correct." },
            { UserNotFound, "The User was not found." },
            { EmailNotVerified, "The email address is not verified." },
            { MobileNotVerified, "Mobile No. is not verified." },
            { RoleExists, "The Role already exists in the database." },
            { IdentityError, "Something went wrong. While processing the request." },

            #endregion

            #region 02 Database

            { NullConnectionString, "The connection string passed was null." },
            { InsertFailed, "Failed to Insert into database." },
            { UpdateFailed, "Failed to Update into database." },
            { DeleteFailed, "Failed to Delete from database." },
            { QueryFailed, "Failed to Query into database." },
            { InvalidId, "The Id passed is invalid." },
            { RecordNotFound, "Record was not found." },

            #endregion

            #region 03 Settings

            { SettingNotFound, "The setting requested does not exists." },
            { SettingTypeInvalid, "The setting type is invalid." },
            { ModulesNotPassed, "No Module details found in request." },
            { FieldsNotPassed, "No Field details found in request." },

            #endregion
        };

        public IDictionary<string, string> ErrorMessages { get; protected set; }


        #region 00 System
        public const string UnknownSystemException = "00SYS001";
        public const string NotImplemented = "00SYS002";
        public const string ArgumentNull = "00SYS003";
        public const string NullValue = "00SYS004";

        #endregion

        #region 01 Authentication
        public const string EmailTaken = "01AU001";
        public const string PasswordNotStrong = "01AU002";
        public const string IncorrectCredentials = "01AU003";
        public const string UserNotFound = "01AU004";
        public const string EmailNotVerified = "01AU005";
        public const string MobileNotVerified = "01AU006";
        public const string RoleExists = "01AU007";
        public const string IdentityError = "01AU008";
        #endregion

        #region 02 Database

        public const string NullConnectionString = "02DB001";
        public const string InsertFailed = "02DB002";
        public const string UpdateFailed = "02DB003";
        public const string DeleteFailed = "02DB004";
        public const string QueryFailed = "02DB005";
        public const string InvalidId = "02DB006";
        public const string RecordNotFound = "02DB007";


        #endregion


        #region 03 Settings

        public const string SettingNotFound = "03SET001";
        public const string SettingTypeInvalid = "03SET002";
        public const string ModulesNotPassed = "03SET003";
        public const string FieldsNotPassed = "03SET004";

        #endregion

    }
}
