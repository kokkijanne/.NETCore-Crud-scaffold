using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using Todo.Api.Middleware.Services;

namespace Todo.Api.Middleware
{
    public class CheckPinCodeAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var pinCodeCheck = context.HttpContext.RequestServices.GetRequiredService<IPinService>();
            var pinCode = context.HttpContext.Request.Headers["Pin"];
            if (!pinCodeCheck.IsCorrect(pinCode))
            {
                context.Result = new StatusCodeResult(401);
            }
        }
    }
}
