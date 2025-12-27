namespace SAP.NET6.Services
{
    public interface IOrderValidationHandler
    {
        void Validate(OrderValidationRequest request);
        IOrderValidationHandler SetNext(IOrderValidationHandler handler);
    }
}