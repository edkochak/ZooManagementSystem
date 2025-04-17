namespace Zoo.Domain.Events
{
    public static class DomainEvents
    {
        private static List<IDomainEventHandler> _handlers = new();
        
        public static void Register(IDomainEventHandler handler)
        {
            if (!_handlers.Contains(handler))
            {
                _handlers.Add(handler);
            }
        }
        
        public static void Unregister(IDomainEventHandler handler)
        {
            if (_handlers.Contains(handler))
            {
                _handlers.Remove(handler);
            }
        }
        
        public static void Raise<T>(T domainEvent) where T : DomainEvent
        {
            foreach (var handler in _handlers)
            {
                if (handler is IDomainEventHandler<T> typedHandler)
                {
                    typedHandler.Handle(domainEvent);
                }
            }
        }
    }
}
