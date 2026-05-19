using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API_;
public sealed class BiluppgifterApiClient
{
    private readonly HttpClient httpClient;

    public BiluppgifterApiClient(string token)
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://data.biluppgifter.se/")
        };

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CarAPI/1.0");
    }

    public async Task<VehicleLookupResult> GetVehicleByRegNoAsync(string regNo, CancellationToken cancellationToken = default)
    {
        var encodedRegNo = Uri.EscapeDataString(regNo.Trim().ToUpperInvariant());
        var call = $"api/v1/lookup/vehicle/regno/{encodedRegNo}";

        using var response = await httpClient.GetAsync(call, cancellationToken);
        var rawJson = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var status = (int)response.StatusCode;
            throw new HttpRequestException($"Fjärrservern returnerade ett fel: ({status}) {response.ReasonPhrase}");
        }

        var json = JObject.Parse(rawJson);

        var result = new VehicleLookupResult
        {
            RegNo = GetString(json, "data.attributes.regno", "data.regno", "vehicle.regno"),
            Type = GetString(json, "data.basic.data.type", "data.type", "data.attributes.type", "vehicle.type"),
            Make = GetString(json, "data.basic.data.make", "data.model.data.make", "vehicle.make"),
            Model = GetString(json, "data.basic.data.model", "data.model.data.model", "vehicle.model"),
            ModelYear = GetString(json, "data.basic.data.model_year", "vehicle.model_year"),
            Color = GetString(json, "data.basic.data.color", "vehicle.color"),
            RawJson = json.ToString(Formatting.Indented)
        };

        if (string.IsNullOrWhiteSpace(result.RegNo))
        {
            result.RegNo = regNo;
        }

        return result;
    }

    private static string GetString(JToken root, params string[] paths)
    {
        foreach (var path in paths)
        {
            var value = root.SelectToken(path)?.ToString();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return string.Empty;
    }
}

public sealed class VehicleLookupResult
{
    public string RegNo { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ModelYear { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string RawJson { get; set; } = string.Empty;
}
