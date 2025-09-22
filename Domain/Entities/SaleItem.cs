using Sales.Domain.Exceptions;
using System;
namespace Sales.Domain.Entities;
public class SaleItem
{
    public Guid Id { get; private set; }
    public string ProductId { get; private set; } = default!; // external id
    public string ProductName { get; private set; } = default!; //denormalization
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public decimal Total => Math.Round(Quantity * UnitPrice * (1 - DiscountPercentage), 2);
    public bool Cancelled { get; private set; }
    private SaleItem() { }
    public SaleItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId ?? throw new
        ArgumentNullException(nameof(productId));
        ProductName = productName ?? throw new
        ArgumentNullException(nameof(productName));
        SetQuantityAndPrice(quantity, unitPrice);
        Cancelled = false;
    }
    public void SetQuantityAndPrice(int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new DomainException("Quantity must be greater than zero.");

        if (unitPrice < 0) throw new DomainException("Unit price cannot benegative.");

        if (quantity > 20) throw new DomainException("Cannot sell more than 20 identical items.");

        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercentage = CalculateDiscountPercentage(quantity);
    }
    private static decimal CalculateDiscountPercentage(int quantity)
    {
        if (quantity < 4) return 0m;
        if (quantity >= 4 && quantity < 10) return 0.10m;
        // 10..20
        return 0.20m;
    }
    public void Cancel()
    {
        Cancelled = true;
    }
}
