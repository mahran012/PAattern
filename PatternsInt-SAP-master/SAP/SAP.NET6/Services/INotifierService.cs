using System.Threading.Tasks;

namespace SAP.NET6.Services
{
    public interface INotifierService
    {
        Task Notify(string message);
    }
}