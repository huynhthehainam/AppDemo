namespace AMG.App.Infrastructure.Models.Settings
{
    public class EmailSettings
    {
        public string SenderName { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
    }
}