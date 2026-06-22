namespace PharmacyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly JsonFileStore _store;

    public MedicinesController(JsonFileStore store)
    {
        _store = store;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
    {
        var medicines = await _store.GetMedicinesAsync();
        return Ok(medicines.OrderBy(m => m.FullName));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Medicine>> GetById(int id)
    {
        var medicines = await _store.GetMedicinesAsync();
        var medicine = medicines.FirstOrDefault(m => m.Id == id);

        if (medicine == null)
            return NotFound();

        return Ok(medicine);
    }

    [HttpPost]
    public async Task<ActionResult<Medicine>> Create([FromBody] Medicine medicine)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var medicines = await _store.GetMedicinesAsync();
        medicine.Id = medicines.Any() ? medicines.Max(x => x.Id) + 1 : 1;
        medicine.Price = Math.Round(medicine.Price, 2);

        medicines.Add(medicine);
        await _store.SaveMedicinesAsync(medicines);

        return CreatedAtAction(nameof(GetById), new { id = medicine.Id }, medicine);
    }
}