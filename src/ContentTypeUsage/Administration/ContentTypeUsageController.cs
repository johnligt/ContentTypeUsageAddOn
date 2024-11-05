using ContentTypeUsage.Models;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Editor;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentTypeUsage.Administration
{
    /// <summary>
    /// Retrieves the data for the Content Type Usage Plugin
    /// </summary>
    [Authorize(Policy = ContentTypeUsageConstants.AuthorizationPolicy)]
    public sealed class ContentTypeUsageController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        private const string ViewPrefix = "~/Views/ContentTypeUsage/";

        public ContentTypeUsageController(IContentLoader contentLoader,
            IContentRepository contentRepository,
            ContentAssetHelper contentAssetHelper)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        [HttpGet]
        [Route("~/contenttypeusage/listcontenttypes")]
        public IActionResult ListContentTypes()
        {
            var contentTypesList = ServiceLocator.Current.GetInstance<IContentTypeRepository>()
               .List()
               .OrderBy(p => p.Name)
               .Select(x => new ContentTypeDisplayModel(x.Name, x.DisplayName, x.ID))
               .ToList();

            return View($"{ViewPrefix}ListContentTypes.cshtml", contentTypesList);
        }

        [HttpGet]
        [Route("~/contenttypeusage/listcontentofcontenttype/{id}")]
        public IActionResult ListContentOfContentType(string id)
        {
            var conversionResult = int.TryParse(id, out var contentTypeId);

            if (conversionResult == false)
            {
                return null;
            }

            var contentModelUsage = ServiceLocator.Current.GetInstance<IContentModelUsage>();

            var contentType = ServiceLocator.Current.GetInstance<IContentTypeRepository>().Load(contentTypeId);
            var usages = contentModelUsage.ListContentOfContentType(contentType).OrderBy(x => x.Name);

            var contentReferences = usages
                .Select(x => x.ContentLink.ToReferenceWithoutVersion())
                .Distinct()
                .ToList();

            var contentList = new List<ContentListInstance>();

            foreach (var contentReference in contentReferences)
            {
                var contentListInstance = CreateContentListInstance(contentReference);

                contentList.Add(contentListInstance);
            }

            var contentListResult = new ContentListResult();

            contentListResult.ContentTypeName = contentType.Name;
            contentListResult.ContentTypeDisplayName = contentType.DisplayName;
            contentListResult.NumberOfItems = contentList.Count;
            contentListResult.ContentList = contentList;

            return View($"{ViewPrefix}ContentListResult.cshtml", contentListResult);
        }

        private ContentListInstance CreateContentListInstance(ContentReference contentReference)
        {
            var contentItem = _contentLoader.Get<IContent>(contentReference);
            var ancestors = _contentRepository.GetAncestors(contentReference).ToList();

            var path = "";

            for (var x = ancestors.Count - 1; x >= 0; x--)
            {
                path += "\\" + ancestors[x].Name;
            }

            var contentListInstance = new ContentListInstance();

            contentListInstance.Id = contentItem.ContentLink.ID;
            contentListInstance.Name = contentItem.Name;
            contentListInstance.Path = path;

            if (contentItem is PageData)
            {
                var url = contentItem.ContentLink.GetFriendlyUrl(true);
                contentListInstance.Url = url;
            }
            else
            {
                var url = PageEditing.GetEditUrl(contentReference);
                contentListInstance.Url = url;

                var assetOwner = _contentAssetHelper.GetAssetOwner(contentReference);

                if (assetOwner == null)
                {
                    return contentListInstance;
                }

                contentListInstance.AssetOwnerName = assetOwner.Name;
                contentListInstance.AssetOwnerUrl = assetOwner.ContentLink.GetFriendlyUrl(true);
            }

            return contentListInstance;
        }
    }
}
