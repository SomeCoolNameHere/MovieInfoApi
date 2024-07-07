using Microsoft.AspNetCore.Mvc;

namespace MovieInfoApi;

public class GlobalConsts
{
    public const string ApiKey = nameof(ApiKey);

    public static ContentResult ErrorAuthResult = new()
    {
        StatusCode = 401,
        Content = "Api Key is not valid"
    };

    public static int DefaultTimeoutValue = 20;
}