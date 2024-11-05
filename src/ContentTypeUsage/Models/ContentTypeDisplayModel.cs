namespace ContentTypeUsage.Models
{
    public class ContentTypeDisplayModel
    {
        public ContentTypeDisplayModel(string name, string displayName, int id)
        {
            Name = name;
            DisplayName = displayName;
            Id = id;
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Id { get; set; }
    }
}
