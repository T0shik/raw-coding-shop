using System.Threading.Tasks;

namespace RawCoding.Shop.UI.Workers.Email
{
    public interface IEmailSink
    {
        ValueTask SendAsync(EmailRequest request);
    }
}