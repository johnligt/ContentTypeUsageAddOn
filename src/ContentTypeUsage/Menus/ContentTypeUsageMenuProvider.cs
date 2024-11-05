using EPiServer.Shell.Navigation;

namespace ContentTypeUsage.Menus;

[MenuProvider]
public sealed class ContentTypeUsageMenuProvider : IMenuProvider
{
    public IEnumerable<MenuItem> GetMenuItems()
    {
        var menuItems = new List<MenuItem>();

        var menu1 = CreateMenuItem("ContentType Usage", "/global/cms/example.menu.four", 
            "/contenttypeusage/listcontenttypes/", SortIndex.Last + 40);

        menuItems.Add(menu1);
       
        return menuItems;
    }

    private static UrlMenuItem CreateMenuItem(string name, string path, string url, int index)
    {
        return new UrlMenuItem(name, path, url)
        {
            IsAvailable = context => true,
            SortIndex = index,
            AuthorizationPolicy = ContentTypeUsageConstants.AuthorizationPolicy
        };
    }
}
