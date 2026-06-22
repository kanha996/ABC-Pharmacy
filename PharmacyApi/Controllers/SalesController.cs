namespace PharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly JsonFileStore _store;

    public SalesController(JsonFileStore store)
    {
        _store = store;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleRecord>>> GetAll()
    {
        var sales = await _store.GetSalesAsync();
        return Ok(sales.OrderByDescending(s => s.SoldOn));
    }


    [HttpPost]
    public async Task<ActionResult<SaleRecord>> Create([FromBody] CreateSaleRequest request)
    {
        if (request.SoldQuantity <= 0)
            return BadRequest("Sold quantity must be greater than zero.");

        var medicines = await _store.GetMedicinesAsync();
        var medicine = medicines.FirstOrDefault(m => m.Id == request.MedicineId);

        if (medicine == null)
            return NotFound("Medicine not found.");

        if (medicine.Quantity < request.SoldQuantity)
            return BadRequest("Insufficient stock.");

        medicine.Quantity -= request.SoldQuantity;

        var sales = await _store.GetSalesAsync();
        var sale = new SaleRecord
        {
            Id = sales.Any() ? sales.Max(x => x.Id) + 1 : 1,
            MedicineId = medicine.Id,
            MedicineName = medicine.FullName,
            SoldQuantity = request.SoldQuantity,
            UnitPrice = medicine.Price,
            TotalAmount = Math.Round(medicine.Price * request.SoldQuantity, 2),
            SoldOn = DateTime.UtcNow
        };

        sales.Add(sale);

        await _store.SaveMedicinesAsync(medicines);
        await _store.SaveSalesAsync(sales);

        return Ok(sale);
    }
}