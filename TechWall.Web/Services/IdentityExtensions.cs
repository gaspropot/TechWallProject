using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace TechWall.Services
{
    public static class IdentityExtensions
    {
        public static string GetUserPicture(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Picture");

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}