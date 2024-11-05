using EPiServer.Shell.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ContentTypeUsage.ServiceExtensions;

public static class ContentTypeUsageServiceExtensions
{
    public static IServiceCollection AddContentTypeUsage(
        this IServiceCollection services,
        Action<AuthorizationOptions>? authorizationOptions = null)
    {
        // Authorization
        if (authorizationOptions != null)
        {
            services.AddAuthorization(authorizationOptions);
        }
        else
        {
            var allowedRoles = new List<string> { "CmsAdmins", "Administrator", "WebAdmins" };
            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(ContentTypeUsageConstants.AuthorizationPolicy, policy =>
                {
                    policy.RequireRole(allowedRoles);
                });
            });
        }

        // If you are extending the CMS Editor Interface with an IFrameComponent, then you will need to declare the module here.
        // This will require a corresponding module.config file in the modules/_protected/ContentTypeUsage folder within the website.
        services.Configure<ProtectedModuleOptions>(
            options =>
            {
                if (!options.Items.Any(x => string.Equals(x.Name, "ContentTypeUsage", StringComparison.OrdinalIgnoreCase)))
                {
                    options.Items.Add(new ModuleDetails { Name = "ContentTypeUsage" });
                }
            });

        return services;
    }

    public static IApplicationBuilder UseContentTypeUsage(this IApplicationBuilder builder)
    {
        // Set up your pipelines here.

        return builder;
    }
}
