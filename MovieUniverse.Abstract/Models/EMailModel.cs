namespace MovieUniverse.Abstract.Models
{
    public class EMailModel
    {
        public string SourceAddres { get; set; } 
        public string DestinationAddress { get; set; }
        public string Subject { get; set; }
        public string MessegeBody { get; set; }
    }
}