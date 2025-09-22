using Microsoft.AspNetCore.Mvc;
using Sales.Application.DTOs;
using Sales.Application.Interfaces;

namespace Sales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _svc;
    public SalesController(ISaleService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SaleDto>>> GetAll()
    {
        var list = await _svc.ListAsync();
        return Ok(list);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> Get(Guid id)
    {
        var sale = await _svc.GetByIdAsync(id);
        if (sale == null)
        {
            return NotFound();
        }
        return Ok(sale);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Create([FromBody] CreateSaleRequest req)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _svc.CreateAsync(req);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SaleDto>> Update(Guid id, [FromBody] CreateSaleRequest req)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var updated = await _svc.UpdateAsync(id, req);
        return Ok(updated);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelSale(Guid id)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _svc.CancelSaleAsync(id);
        return NoContent();
    }
    [HttpPost("{id}/items/{productId}/cancel")]
    public async Task<IActionResult> CancelItem(Guid id, string productId)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _svc.CancelItemAsync(id, productId);
        return NoContent();
    }
}