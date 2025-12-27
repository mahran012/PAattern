namespace SAP.NET6.Services
{
    public abstract class OrderValidationHandler : IOrderValidationHandler
    {
        private IOrderValidationHandler _nextHandler;

        public IOrderValidationHandler SetNext(IOrderValidationHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Validate(OrderValidationRequest request)
        {
            if (_nextHandler != null)
            {
                _nextHandler.Validate(request);
            }
        }
    }
}