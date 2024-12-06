namespace KalakariWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Website
    {
        [Key]
        public int? Id { get; set; } = 0;
        public string WebsiteName { get; set; }
        public string WebsiteType { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Interests { get; set; }
        public string? ProductCategories { get; internal set; }
    }
    public class Blog
    {
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
    }

}
