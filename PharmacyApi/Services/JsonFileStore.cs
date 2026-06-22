using System.Text.Json;

namespace PharmacyApi.Services;

public class JsonFileStore
{
    private readonly IWebHostEnvironment _env;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private string MedicinesPath => Path.Combine(_env.ContentRootPath, "Data", "medicines.json");
    private string SalesPath => Path.Combine(_env.ContentRootPath, "Data", "sales.json");

    public JsonFileStore(IWebHostEnvironment env)
    {
        _env = env;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
    }

    public async Task<List<Medicine>> GetMedicinesAsync()
    {
        await EnsureFileAsync(MedicinesPath);
        var json = await File.ReadAllTextAsync(MedicinesPath);
        return JsonSerializer.Deserialize<List<Medicine>>(json, _jsonOptions) ?? new List<Medicine>();
    }

    public async Task SaveMedicinesAsync(List<Medicine> medicines)
    {
        await _lock.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(medicines, _jsonOptions);
            await File.WriteAllTextAsync(MedicinesPath, json);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<List<SaleRecord>> GetSalesAsync()
    {
        await EnsureFileAsync(SalesPath);
        var json = await File.ReadAllTextAsync(SalesPath);
        return JsonSerializer.Deserialize<List<SaleRecord>>(json, _jsonOptions) ?? new List<SaleRecord>();
    }

    public async Task SaveSalesAsync(List<SaleRecord> sales)
    {
        await _lock.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(sales, _jsonOptions);
            await File.WriteAllTextAsync(SalesPath, json);
        }
        finally
        {
            _lock.Release();
        }
    }

    private static async Task EnsureFileAsync(string path)
    {
        var dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(path))
            await File.WriteAllTextAsync(path, "[]");
    }
}