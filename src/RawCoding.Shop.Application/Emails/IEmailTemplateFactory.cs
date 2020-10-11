using System.Threading.Tasks;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Application.Emails
{
    public interface IEmailTemplateFactory
    {
        public Task<string> RenderOrderAsync(Order order);
    }
}