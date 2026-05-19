using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace m05.Biluppgifter;

//Skapa Api-klienten för biluppgifter
//  Anropa och returnera resultat
public sealed class API_C
{
    private readonly HttpClient _httpClient;

    // Initiera Http-klient och sätt headers för API-anrop
    public API_C(string token)
    {
        // Skapa Http-klient och sätt basadress för API
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://data.biluppgifter.se/")
        };

        // Autentisering & nödvanliga headers
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("m05CarApp/1.0");
    }

    // Hämta fordon via RegNr
    public async Task<VehicleLookupResult> GetVehicleByRegNoAsync(string regNo,
        CancellationToken cancellationToken = default)
    {
        // Rensa och URL-koda RegNr
        var encodedRegNo = Uri.EscapeDataString(regNo.Trim().ToUpperInvariant());
        var call = $"api/v1/lookup/vehicle/regno/{encodedRegNo}";

        // Skicka en GET-anrop & läs svar
        using var response = await _httpClient.GetAsync(call, cancellationToken);
        var rawJson = await response.Content.ReadAsStringAsync(cancellationToken);

        // Kontrollera om anropet misslyckades
        if (!response.IsSuccessStatusCode)
        {
            var status = (int)response.StatusCode;
            throw new HttpRequestException($"Remote error: ({status}) {response.ReasonPhrase}");
        }

        // Parsa json strängen
        var json = JObject.Parse(rawJson);

        // Hämta data från JSON-paths
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

        // Använd den inmatade RegNr om API saknar det
        if (string.IsNullOrWhiteSpace(result.RegNo)) result.RegNo = regNo;
        return result;
    }

    // Hämta JSON value från paths
    private static string GetString(JToken root, params string[] paths)
    {
        // Loopa igenom alla paths
        foreach (var path in paths)
        {
            // hämta value
            var value = root.SelectToken(path)?.ToString();
            if (!string.IsNullOrWhiteSpace(value)) return value;
        }

        return string.Empty;
    }
}

// En förmulär, för att lagra resultatet från API
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