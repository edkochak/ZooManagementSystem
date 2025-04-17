namespace Zoo.Domain.Events
{
    public interface IDomainEventHandler
    {
    }

    public interface IDomainEventHandler<T> : IDomainEventHandler where T : DomainEvent
    {
        void Handle(T domainEvent);
    }
}
