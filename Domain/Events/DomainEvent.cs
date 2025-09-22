using System;

namespace Sales.Domain.Events;

public record DomainEvent(DateTime OccurredAt = default)
{
    public DateTime When => OccurredAt == default ? DateTime.UtcNow : OccurredAt;
}