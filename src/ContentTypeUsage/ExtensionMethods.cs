using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace ContentTypeUsage
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get the friendly url for a given reference
        /// </summary>
        /// <param name="contentReference"></param>
        /// <param name="includeHost">Does the full hostname need to be included</param>
        /// <returns>A string with the friendly url</returns>
        public static string GetFriendlyUrl(this ContentReference contentReference, bool includeHost = false)
        {
            if (ContentReference.IsNullOrEmpty(contentReference))
            {
                return string.Empty;
            }

            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var url = urlResolver.GetUrl(contentReference);

            //TODO ???
            //return includeHost ? url.GetExternalUrl() : url;
            return url;
        }

        /// <summary>
        /// Converts a relative url to an absolute url
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public static string GetExternalUrl(this string input)
        //{
        //    if (string.IsNullOrWhiteSpace(input))
        //    {
        //        return string.Empty;
        //    }

        //    if (input.StartsWith("http"))
        //    {
        //        return input;
        //    }

        //    if (!input.StartsWith("/"))
        //    {
        //        input = "/" + input;
        //    }

        //    ///private static readonly IHttpContextAccessor _httpContextAccessor;

        //    //var httpContext = Context.GetHttpContext();

        //    var httpContextAccessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();

        //    var siteUri = httpContextAccessor.HttpContext?.Request.GetDisplayUrl() != null
        //        ? HttpContext.Current.Request.Url
        //        : SiteDefinition.Current.SiteUrl;

        //    if (siteUri == null)
        //    {
        //        return input;
        //    }

        //    var urlBuilder = new UrlBuilder(input)
        //    {
        //        Scheme = siteUri.Scheme,
        //        Host = siteUri.Host,
        //        Port = siteUri.Port
        //    };

        //    // Correct for SSL-strips like a load balancer
        //    if (HttpContext.Current?.Request.Headers["X-Forwarded-Proto"]?.Equals("https", StringComparison.OrdinalIgnoreCase) == true)
        //    {
        //        urlBuilder.Scheme = "https";
        //        urlBuilder.Port = 443;
        //    }

        //    return urlBuilder.ToString();
        //}

        
        
    }
}
