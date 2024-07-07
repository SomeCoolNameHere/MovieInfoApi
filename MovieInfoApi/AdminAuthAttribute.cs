using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MovieInfoApi;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminAuthAttribute : Attribute, IAsyncActionFilter
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(GlobalConsts.ApiKey, out var extractedApiKey))
        {
            context.Result = GlobalConsts.ErrorAuthResult;
            return;
        }

        var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>(GlobalConsts.ApiKey);
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = GlobalConsts.ErrorAuthResult;
            return;
        }

        await next();
    }
}