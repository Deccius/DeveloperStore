namespace Sales.Application.DTOs
{
    public class SaleItemDto
    {
        public string ProductId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Total { get; set; }

        public bool Cancelled { get; set; }
    }
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = default!;
        public DateTime Date { get; set; }
        public string CustomerId { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public string BranchId { get; set; } = default!;
        public string BranchName { get; set; } = default!;
        public bool Cancelled { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
