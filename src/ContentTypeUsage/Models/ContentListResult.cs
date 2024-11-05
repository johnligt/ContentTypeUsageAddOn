namespace ContentTypeUsage.Models
{
    public class ContentListResult
    {

        public string ContentTypeName { get; set; }
        public string ContentTypeDisplayName { get; set; }
        public int NumberOfItems { get; set; }
        public List<ContentListInstance> ContentList { get; set; }
    }

    public class ContentListInstance
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }
        public string AssetOwnerUrl { get; set; }
        public string AssetOwnerName { get; set; }
    }
}
