using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Todo.WebApp.Filters
{
    public class AuthFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.Keys.Contains("token") == false)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
                return;
            }
        }
    }
}
