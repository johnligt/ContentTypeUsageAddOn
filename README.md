# Optimizely 12 Contenttype Usage Addon

An addon for Optimizely 12, to list the pages and blocks using a selected contenttype.
The items in the list link directly to the pages and blocks.

![contenttypelist](https://github.com/user-attachments/assets/64087b86-a7ac-45c4-b9a3-665aebbbaebe)

![usagelist](https://github.com/user-attachments/assets/a029cc1a-1ad1-4826-816f-a8bd64628a17)

Add 
```services.AddContentTypeUsage(); ``` 
to Startup.cs ConfigureServices

Add 
```app.UseContentTypeUsage(); ``` 
to Startup.cs Configure

To configure authorization (not required) use code like this:

```cs
var allowedRoles = new List<string> { "CmsAdmins", "Administrator", "WebAdmins", "WebEditors" };
        
var authorizationOptionsAction = new Action<AuthorizationOptions>(authorizationOptions =>
{
    authorizationOptions.AddPolicy(ContentTypeUsageConstants.AuthorizationPolicy, policy =>
    {
        policy.RequireRole(allowedRoles);
    });
});

// Adding specific authorization options is only required if the default
// of { "CmsAdmins", "Administrator", "WebAdmins"} is not enough.
services.AddContentTypeUsage(authorizationOptionsAction);

```
