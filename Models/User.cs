namespace KalakariWeb.Models
{
    public class User
    {
        public int Id { get; set; }               // Integer Id
        public Guid UserId { get; set; }           // GUID UserId
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
