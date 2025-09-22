namespace Sales.Application.DTOs;
public class CreateSaleItemRequest
{
    public string ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
public class CreateSaleRequest
{
    public string SaleNumber { get; set; } = default!;
    public DateTime Date { get; set; }
    public string CustomerId { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

