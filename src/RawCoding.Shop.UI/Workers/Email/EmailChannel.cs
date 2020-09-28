using System.Threading.Channels;
using System.Threading.Tasks;

namespace RawCoding.Shop.UI.Workers.Email
{
    public class EmailChannel : IEmailSink, IEmailQueue
    {
        private readonly Channel<EmailRequest> _channel;

        public EmailChannel()
        {
            _channel = Channel.CreateUnbounded<EmailRequest>();
        }

        public ValueTask SendAsync(EmailRequest request)
        {
            return _channel.Writer.WriteAsync(request);
        }

        public ValueTask<EmailRequest> ReadAsync()
        {
            return _channel.Reader.ReadAsync();
        }
    }
}