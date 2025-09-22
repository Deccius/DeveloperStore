using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.Domain.Entities;

namespace Sales.Domain.Repositories;
public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(Guid id);
    Task<Sale?> GetBySaleNumberAsync(string saleNumber);
    Task<IReadOnlyList<Sale>> ListAsync();
    Task AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(Guid id);
}