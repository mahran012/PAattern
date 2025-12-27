using ExampleEmailSdk;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public class EmailNotifierAdapter : INotifier
    {
        private readonly EmailNotifier _emailNotifier;
        private readonly string _receiver;

        public EmailNotifierAdapter(string receiver)
        {
            _emailNotifier = new EmailNotifier();
            _receiver = receiver;
        }

        public Task NotifyAsync(string message)
        {
            _emailNotifier.SendEmail(_receiver, "Web Shop Notification", message);
            return Task.CompletedTask;
        }
    }
}