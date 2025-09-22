namespace Sales.Domain.Events;

public record SaleCreatedEvent(Guid SaleId, string SaleNumber) : DomainEvent;
public record SaleModifiedEvent(Guid SaleId, string SaleNumber) : DomainEvent;
public record SaleCancelledEvent(Guid SaleId, string SaleNumber) : DomainEvent;
public record ItemCancelledEvent(Guid SaleId, Guid ItemId, string ProductId) : DomainEvent;