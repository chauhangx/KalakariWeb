
namespace KalakariWeb.Models
{
    public class WebCustomization
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } // Represents userEmail from the payload
        public string Header { get; set; } // Represents header from the payload
        public string Topic { get; set; } // Represents topic from the payload
        public string Author { get; set; } // Represents author from the payload
        public string Image { get; set; } // Represents image from the payload
        public string Content { get; set; } // Represents content from the payload
        public bool? IsActive { get; set; } // Represents isActive from the payload
        public string Footer { get; set; } // Optional property retained from the existing class
        public DateTime InsertedOn { get; set; } // Tracks when the record was inserted
        public DateTime ModifiedOn { get; set; } // Tracks when the record was last modified
    }

    public class CustomizationDto
    {
        public int Id { get; set; } // For updates
        public string UserEmail { get; set; } // For new records
        public string Header { get; set; }
        public string Topic { get; set; }
        public string Footer { get; set; }
        public bool? IsActive { get; set; } // Nullable for optional inclusion
    }
}
