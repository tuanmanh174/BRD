using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Common.Infrastructure
{
    public class Constants
    {
        public class ColumnDefines
        {
            public static readonly string[] Account = {
            "Username",
            "Type",
        };
        }
        public class Settings
        {
            public const string FileSettings = "Settings";
            public const string DeviceTypes = "DeviceTypes";
            public const string DefaultCulture = "Cultures:Default";
            public const string OptionCulture = "Cultures:Option";
            public const string PageSize = "Pagination:PageSize";
            public const string LoginExpiredTime = "Login:ExpiredTime";
            public const string DefaultConnection = "DefaultConnection";
            public const string ResourcesDir = "Resources";
            public const string JwtSection = "jwt";
            public const string Cultures = "Cultures";
            public const string BRDDataAccess = "BRD.DataAccess";
            public const int MaxUserSendToIcu = 5;

            public const string DefaultExpiredTime = "60";
            public const string EncryptKey = "Encryptor:Key";
            public const string MaxTimezoneTimeHour = "23";
            public const string MaxTimezoneTimeMinute = "59";
            public const double MaxTimezoneTimeHourMinute = 23.99;
            public const string ExpiredSessionTime = "ExpiredSessionTime";
            public const string DefaultAccountUsername = "DefaultAccount:Username";
            public const string DefaultAccountPassword = "DefaultAccount:Password";
            public const string DefaultAccountPhone = "DefaultAccount:Phone";
            public const string DomainFrontEnd = "Domain:FrontEnd";
            public const string MailSettings = "MailSettings";
            public const string MailDevelopSettings = "MailDevelopSettings";
            public const string ExternalAccountProvider = "ExternalAccountProvider";
        }
    }
}
