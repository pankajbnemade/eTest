using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;

namespace ERP.Models.Logger
{
    public class SeriLogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments.Any())
            {
                filterContext.HttpContext.Session.SetString("SeriLogActionArguments", JsonConvert.SerializeObject(filterContext.ActionArguments));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
