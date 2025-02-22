using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Raven.Client;

namespace WebApi.App_Start
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextInitializeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // I am adding these lines of code because I want to exit/do nothing if processing endpoint products or orders
            var routeTemplate = actionExecutedContext.ActionContext.ControllerContext.RouteData.Route.RouteTemplate;

            if (string.IsNullOrEmpty(routeTemplate))
            {
                throw new ArgumentNullException(nameof(routeTemplate), "Route template cannot be null.");
            }

            if (new[] { "products", "orders" }.Any(route => routeTemplate.Contains(route)))
            {
                return;
            }

            var container = GlobalConfiguration.Configuration.DependencyResolver;
            var method = actionExecutedContext.Request.Method;
            if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete)
            {
                var session = (IDocumentSession)container.GetService(typeof(IDocumentSession));
                session.SaveChanges();
            }
        }
    }
}