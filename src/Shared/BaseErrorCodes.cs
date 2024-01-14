// ReSharper disable InconsistentNaming
#pragma warning disable CS1591
global using System;
global using System.Collections.Generic;
global using System.Threading.Tasks;


namespace Shared
{
    public class BaseErrorCodes
    {
        public BaseErrorCodes()
        {
            ErrorMessages = _baseErrorMessages;
        }

        protected IDictionary<string, string> _baseErrorMessages = new Dictionary<string, string>
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

            { NullConnectionString, "Could not connect to the database." },
            { InsertFailed, "Failed to Insert into database." },
            { UpdateFailed, "Failed to Update into database." },
            { DeleteFailed, "Failed to Delete from database." },
            { QueryFailed, "Failed to Query into database." },
            { InvalidId, "The Id passed is invalid." },
            { RecordNotFound, "Record was not found." },
            { DatabaseUnknownError, "Some problem occurred with our database systems." },

            #endregion

            { FileNotFound, "Requested file was not found in the file system." },
            { FileNotPlayable, "Requested file could not be played at the moment." },

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

        public const string DatabaseUnknownError = "02DB000";
        public const string NullConnectionString = "02DB001";
        public const string InsertFailed = "02DB002";
        public const string UpdateFailed = "02DB003";
        public const string DeleteFailed = "02DB004";
        public const string QueryFailed = "02DB005";
        public const string InvalidId = "02DB006";
        public const string RecordNotFound = "02DB007";


        #endregion

        public const string FileNotFound = "03DRV001";
        public const string FileNotPlayable = "03DRV002";
    }
}
