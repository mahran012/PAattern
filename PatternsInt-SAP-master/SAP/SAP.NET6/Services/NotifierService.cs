using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public class NotifierService : INotifierService
    {
        private readonly INotifier _smsNotifier;
        private readonly INotifier _emailNotifier;

        public NotifierService(IOptionsMonitor<NotificationsConfiguration> notificationOptions)
        {
            var config = notificationOptions.CurrentValue;
            _smsNotifier = new SmsNotifierAdapter(config.SmsReceiver);
            _emailNotifier = new EmailNotifierAdapter(config.EmailReceiver);
        }

        public async Task Notify(string message)
        {
            // Send notification via both SMS and Email
            await Task.WhenAll(
                _smsNotifier.NotifyAsync(message),
                _emailNotifier.NotifyAsync(message)
            );
        }
    }
}