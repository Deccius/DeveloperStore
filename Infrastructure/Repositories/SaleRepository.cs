using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Repositories;
using Sales.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
namespace Sales.Infrastructure.Repositories;
public class SaleRepository : ISaleRepository
{
    private readonly SalesDbContext _db;
    public SaleRepository(SalesDbContext db) { _db = db; }
    public async Task AddAsync(Sale sale)
    {
        _db.Sales.Add(sale);

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var s = await _db.Sales.FindAsync(id);

        if (s == null) return;

        _db.Sales.Remove(s);

        await _db.SaveChangesAsync();
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _db.Sales.Include("Items").FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber)
    {
        return await _db.Sales.Include("Items").FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
    }

    public async Task<IReadOnlyList<Sale>> ListAsync()
    {
        return await _db.Sales.Include("Items").ToListAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        var trackedSale = await _db.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == sale.Id);


        if (trackedSale == null)
        {
            _db.Sales.Add(sale);
        }

        else
        {
            _db.Entry(trackedSale).CurrentValues.SetValues(sale);

            foreach (var updatedItem in sale.Items)
            {
                if (updatedItem.Id == Guid.Empty)
                {
                    trackedSale.Items.Add(updatedItem);
                    _db.Entry(updatedItem).State = EntityState.Added;
                }
                else
                {
                    var existing = trackedSale.Items.FirstOrDefault(i => i.Id == updatedItem.Id);
                    if (existing != null)
                    {
                        _db.Entry(existing).CurrentValues.SetValues(updatedItem);
                    }
                }
            }
        }
        await _db.SaveChangesAsync();
    }
}
