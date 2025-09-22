using Sales.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Sales.Application.Interfaces;
public interface ISaleService
{
    Task<SaleDto> CreateAsync(CreateSaleRequest request);
    Task<SaleDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<SaleDto>> ListAsync();
    Task<SaleDto> UpdateAsync(Guid id, CreateSaleRequest request);
    Task CancelSaleAsync(Guid id);
    Task CancelItemAsync(Guid id, string productId);
}
