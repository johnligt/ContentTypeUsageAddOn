using AlloyTest.Extensions;
using ContentTypeUsage;
using ContentTypeUsage.ServiceExtensions;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Authorization;

namespace AlloyTest;

public class Startup
{
    private readonly IWebHostEnvironment _webHostingEnvironment;

    public Startup(IWebHostEnvironment webHostingEnvironment)
    {
        _webHostingEnvironment = webHostingEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        services
            .AddCmsAspNetIdentity<ApplicationUser>()
            .AddCms()
            .AddAlloy()
            .AddAdminUserRegistration()
            .AddEmbeddedLocalization<Startup>();

        // Required by Wangkanai.Detection
        services.AddDetection();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
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
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Required by Wangkanai.Detection
        app.UseDetection();
        app.UseSession();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();        
        app.UseContentTypeUsage();
       
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapContent();
        });
    }
}
