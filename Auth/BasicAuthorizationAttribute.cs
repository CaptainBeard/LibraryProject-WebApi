using Microsoft.AspNetCore.Authorization;
public class BasicAuthorizationAttribute : AuthorizeAttribute
{
    public BasicAuthorizationAttribute()
    {
        Policy = "BasicAuthentication";
    }
}