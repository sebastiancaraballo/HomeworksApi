using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Homeworks.DataAccess;
using Homeworks.BusinessLogic;

namespace Homeworks.WebApi.Filters {

    public class ProtectFilter : Attribute, IActionFilter
    {
        private readonly string _role;

        public ProtectFilter(string role) 
        {
            _role = role;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (token == null) {
                context.Result = new ContentResult()
                {
                    Content = "Token is required",
                };
            }
            using (var sessions = new SessionLogic(new UserRepository(ContextFactory.GetNewContext()))) {
                if (!sessions.IsValidToken(token)) {
                    context.Result = new ContentResult()
                    {
                        Content = "Invalid Token",
                    };
                }
                if (!sessions.HasLevel(token, _role)) {
                    context.Result = new ContentResult()
                    {
                        Content = "The user isen't " + _role,
                    };   
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }

}