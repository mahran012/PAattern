using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public interface INotifier
    {
        Task NotifyAsync(string message);
    }
}