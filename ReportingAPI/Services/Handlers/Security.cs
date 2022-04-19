using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ReportingApi.Services.Handlers
{
    public static class Security
    {
        public static void GetUserNameDomain(ClaimsPrincipal principal, out string user_name , out string user_domain) 
        {
            user_name = principal.Identity.Name;
            user_domain = principal.Identity.Name;
            if (principal.Identity.Name != null)
            {
                int hasDomain = principal.Identity.Name.IndexOf(@"\");
                if (hasDomain > 0)
                {
                    user_domain = principal.Identity.Name.Substring(0, hasDomain);
                    user_name = principal.Identity.Name.Remove(0, hasDomain + 1);
                }
            }
        }
        public static bool IsInGroup(this ClaimsPrincipal User, string GroupName)
        {
            var groups = new List<string>();

            var wi = (WindowsIdentity)User.Identity;
#pragma warning disable CA1416 // Проверка совместимости платформы
            if (wi.Groups != null)
#pragma warning restore CA1416 // Проверка совместимости платформы
            {
#pragma warning disable CA1416 // Проверка совместимости платформы
                foreach (var group in wi.Groups)
#pragma warning restore CA1416 // Проверка совместимости платформы
                {
                    try
                    {
#pragma warning disable CA1416 // Проверка совместимости платформы
                        groups.Add(item: group.Translate(typeof(NTAccount))
#pragma warning restore CA1416 // Проверка совместимости платформы
                            .ToString());
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                return groups.Contains(GroupName);
            }
            return false;
        }
    }
}
