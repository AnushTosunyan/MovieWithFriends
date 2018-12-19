namespace MovieUniverse.API.Models
{
    public class UserSessionModel
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public bool IsAuthenticated { get; set; }
        
    }
}