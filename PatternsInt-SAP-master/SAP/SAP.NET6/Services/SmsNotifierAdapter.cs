using ExampleSmsSdk;
using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public class SmsNotifierAdapter : INotifier
    {
        private readonly SmsNotifier _smsNotifier;
        private readonly string _receiver;

        public SmsNotifierAdapter(string receiver)
        {
            _smsNotifier = new SmsNotifier();
            _receiver = receiver;
        }

        public Task NotifyAsync(string message)
        {
            _smsNotifier.SendSms(_receiver, message);
            return Task.CompletedTask;
        }
    }
}