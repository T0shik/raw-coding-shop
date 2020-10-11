using System.Threading.Tasks;

namespace RawCoding.Shop.Application.Emails
{
    public interface IEmailSink
    {
        ValueTask SendAsync(SendEmailRequest request);
    }
}