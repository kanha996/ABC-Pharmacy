using System.ComponentModel.DataAnnotations;
using PharmacyApi.Models;

namespace PharmacyApi.Models;

public class SaleRecord
{
    public int Id { get; set; }

    [Required]
    public int MedicineId { get; set; }

    [Required]
    public string MedicineName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int SoldQuantity { get; set; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal UnitPrice { get; set; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal TotalAmount { get; set; }

    public DateTime SoldOn { get; set; }
}

public class CreateSaleRequest
{
    public int MedicineId { get; set; }
    public int SoldQuantity { get; set; }
}