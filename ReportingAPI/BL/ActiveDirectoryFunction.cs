using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.BL
{
    public static class ActiveDirectoryFunction
    {
        public static bool CheckUserMail(string mail)
        {
            //139.53.2.13:389
            //var parentEntry = new DirectoryEntry("LDAP://" + Environment.UserDomainName);
            var parentEntry = new DirectoryEntry("LDAP://EUROPE");

            var directorySearch = new DirectorySearcher(parentEntry);

            directorySearch.Filter = "(&(objectClass=user)(anr=" + mail +"))";
          //  directorySearch.PropertiesToLoad.Add("mail");

            var user = directorySearch.FindOne();
            if (user is null)
                return false;

            // var mail = user.Properties["mail"][0].ToString();

            return true;
        }
    }
}
