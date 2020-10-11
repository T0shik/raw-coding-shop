using System.Threading.Tasks;
using RawCoding.Shop.Application.Emails;

namespace RawCoding.Shop.UI.Workers.Email
{
    public interface IEmailQueue
    {
        ValueTask<SendEmailRequest> ReadAsync();
    }
}