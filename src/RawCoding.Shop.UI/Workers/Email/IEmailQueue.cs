using System.Threading.Tasks;

namespace RawCoding.Shop.UI.Workers.Email
{
    public interface IEmailQueue
    {
        ValueTask<EmailRequest> ReadAsync();
    }
}