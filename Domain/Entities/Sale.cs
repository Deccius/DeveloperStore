using Sales.Domain.Events;
using Sales.Domain.Exceptions;
namespace Sales.Domain.Entities;
public class Sale
{
    public Guid Id { get; private set; }
    public string SaleNumber { get; private set; } = default!;
    public DateTime Date { get; private set; }

    public string CustomerId { get; private set; } = default!;
    public string CustomerName { get; private set; } = default!;
    public string BranchId { get; private set; } = default!;
    public string BranchName { get; private set; } = default!;

    public List<SaleItem> Items = new();

    public decimal Total => Math.Round(Items.Where(i => !i.Cancelled).Sum(i =>i.Total), 2);
    public bool Cancelled { get; private set; }

    private readonly List<DomainEvent> _events = new();
    public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();
    private Sale() 
    { 
    
    }

    public Sale(string saleNumber, DateTime date, string customerId, string customerName, string branchId, string branchName)
    {
        Id = Guid.NewGuid();

        SaleNumber = saleNumber ?? throw new ArgumentNullException(nameof(saleNumber));

        Date = date;

        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));

        CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));

        BranchId = branchId ?? throw new ArgumentNullException(nameof(branchId));

        BranchName = branchName ?? throw new ArgumentNullException(nameof(branchName));

        _events.Add(new SaleCreatedEvent(Id, SaleNumber));
    }

    public void UpdateSale(string saleNumber, DateTime date, string customerId, string customerName, string branchId, string branchName)
    {
        SaleNumber = saleNumber;
        Date = date;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;

        _events.Add(new SaleModifiedEvent(Id, SaleNumber));
    }

    public void AddOrUpdateItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        if (Cancelled) throw new DomainException("Cannot add items to a cancelled sale.");

        var existing = Items.Find(i => i.ProductId == productId && !i.Cancelled);

        if (existing != null)
        {
            var newQty = existing.Quantity + quantity;

            existing.SetQuantityAndPrice(newQty, unitPrice);
        }
        else
        {
            var item = new SaleItem(productId, productName, quantity, unitPrice);

            Items.Add(item);
        }
        _events.Add(new SaleModifiedEvent(Id, SaleNumber));
    }

    public void UpdateItemQuantity(string productId, int newQuantity)
    {
        var existing = Items.Find(i => i.ProductId == productId && ! i.Cancelled);

        if (existing == null) throw new DomainException("Item not found.");

        existing.SetQuantityAndPrice(newQuantity, existing.UnitPrice);

        _events.Add(new SaleModifiedEvent(Id, SaleNumber));
    }
    public void CancelItem(string productId)
    {
        var existing = Items.Find(i => i.ProductId == productId && !i.Cancelled);

        if (existing == null) throw new DomainException("Item not found or already cancelled.");

        existing.Cancel();

        _events.Add(new ItemCancelledEvent(Id, existing.Id, existing.ProductId));
    }
    public void CancelSale()
    {
        if (Cancelled) return;

        Cancelled = true;

        foreach (var item in Items) item.Cancel();

        _events.Add(new SaleCancelledEvent(Id, SaleNumber));
    }
    public void ClearEvents() => _events.Clear();
}
