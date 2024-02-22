using MyDent.Domain.Models;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using MyDent.Domain.Enum;
using Microsoft.AspNetCore.Http;
using IAuthorizationFilter = Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyDent.Services.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private List<UserRole> Roles { get; set; }

        public AuthorizeAttribute(params UserRole[] roles)
        {
            Roles = roles.ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                if (!Roles.Contains(user.Role))
                {
                    context.Result = new JsonResult(new { message = "Forbidden" })
                    { StatusCode = StatusCodes.Status403Forbidden };
                }
            }
        }
    }
}
