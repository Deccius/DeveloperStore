using Microsoft.Extensions.Logging;
using Sales.Application.DTOs;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Events;
using Sales.Domain.Repositories;
namespace Sales.Application.Services;
public class SaleService : ISaleService
{
    private readonly ISaleRepository _repo;
    private readonly ILogger<SaleService> _logger;
    public SaleService(ISaleRepository repo, ILogger<SaleService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IReadOnlyList<SaleDto>> ListAsync()
    {
        var list = await _repo.ListAsync();

        return list.Select(MapToDto).ToList();
    }

    public async Task<SaleDto> CreateAsync(CreateSaleRequest request)
    {
        var sale = new Sale(request.SaleNumber, request.Date, request.CustomerId, request.CustomerName, request.BranchId, request.BranchName);

        foreach (var it in request.Items)
        {
            sale.AddOrUpdateItem(it.ProductId, it.ProductName, it.Quantity, it.UnitPrice);
        }

        await _repo.AddAsync(sale);

        PublishEvents(sale.Events);

        sale.ClearEvents();

        return MapToDto(sale);
    }
    public async Task<SaleDto> GetByIdAsync(Guid id)
    {
        var sale = await _repo.GetByIdAsync(id);

        if (sale == null) throw new KeyNotFoundException("Sale not found");

        return MapToDto(sale);
    }

    public async Task<SaleDto> UpdateAsync(Guid id, CreateSaleRequest request)
    {
        var sale = await _repo.GetByIdAsync(id);

        if (sale == null) throw new KeyNotFoundException("Sale not found");

        if (sale.Cancelled) throw new InvalidOperationException("Cannot update cancelled sale.");

        sale.UpdateSale(request.SaleNumber, request.Date, request.CustomerId, request.CustomerName, request.BranchId, request.BranchName);

        foreach (var it in request.Items)
        {
            sale.AddOrUpdateItem(it.ProductId, it.ProductName, it.Quantity, it.UnitPrice);
        }

        await _repo.UpdateAsync(sale);

        PublishEvents(sale.Events);

        sale.ClearEvents();

        return MapToDto(sale);
    }
    public async Task CancelSaleAsync(Guid id)
    {
        var sale = await _repo.GetByIdAsync(id);

        if (sale == null) throw new KeyNotFoundException("Sale not found");

        sale.CancelSale();

        await _repo.UpdateAsync(sale);

        PublishEvents(sale.Events);

        sale.ClearEvents();
    }
    public async Task CancelItemAsync(Guid id, string productId)
    {
        var sale = await _repo.GetByIdAsync(id);

        if (sale == null) throw new KeyNotFoundException("Sale not found");

        sale.CancelItem(productId);

        await _repo.UpdateAsync(sale);

        PublishEvents(sale.Events);

        sale.ClearEvents();
    }

    private void PublishEvents(IEnumerable<DomainEvent> events)
    {
        foreach (var ev in events)
        {
            switch (ev)
            {
                case SaleCreatedEvent s:
                    _logger.LogInformation("Event: SaleCreated { SaleId} { SaleNumber}", s.SaleId, s.SaleNumber); 
                    break;

                case SaleModifiedEvent s:
                    _logger.LogInformation("Event: SaleModified { SaleId} { SaleNumber}", s.SaleId, s.SaleNumber); 
                    break;

                case SaleCancelledEvent s:
                    _logger.LogInformation("Event: SaleCancelled { SaleId}{ SaleNumber}", s.SaleId, s.SaleNumber); 
                    break;

                case ItemCancelledEvent i:
                    _logger.LogInformation("Event: ItemCancelled { SaleId} { ItemId} { ProductId}", i.SaleId, i.ItemId, i.ProductId);
                break;

                default:
                    _logger.LogInformation("Event: {Event}", ev.GetType().Name); break;
            }
        }
    }
    private static SaleDto MapToDto(Sale sale)
    {
        return new SaleDto
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            Date = sale.Date,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Cancelled = sale.Cancelled,
            Total = sale.Total,
            Items = sale.Items.Select(i => new SaleItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountPercentage = i.DiscountPercentage,
                Cancelled = i.Cancelled,
                Total = i.Total
            }).ToList()
        };
    }
}