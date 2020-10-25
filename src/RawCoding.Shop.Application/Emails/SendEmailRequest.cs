namespace RawCoding.Shop.Application.Emails
{
    public class SendEmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool Html { get; set; } = true;
    }
}