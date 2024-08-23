using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Enums;
using OpenPrintServerVueNet.Server.Responses;

namespace OpenPrintServerVueNet.Server.Middlewares
{
    public class InstallMiddleware
    {
        private readonly RequestDelegate _next;

        public InstallMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ApplicationContext applicationContext)
        {
            if (httpContext.Request.Path != new PathString("/"))
            {
                if (!httpContext.Request.Path.StartsWithSegments("/api/install"))
                {
                    var isInstalled = applicationContext.Config.FirstOrDefault(conf => conf.Key == ConfigEnum.IsInstalled); // todo maybe delete this
                    if (isInstalled == null || isInstalled.Value != "true") // todo maybe delete this
                    {
                        httpContext.Response.StatusCode = 403;
                        await httpContext.Response.WriteAsJsonAsync(
                            new AppNotInstalledDTO()
                            {
                                IsInstalled = false,
                                Error = "Application is not installed"
                            });
                        return;
                    }
                }
            }
            


            await _next(httpContext);
        }
    }
}