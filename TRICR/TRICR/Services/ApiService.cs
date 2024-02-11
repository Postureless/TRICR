using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TRICR.Services
{
    public class ApiService 
    {
        private readonly HttpClient _httpClient = new();
        public async Task<PopularDirectionsData?> GetPopularDirectionsData(string origin, string currency, string token)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                var apiUrl =
                    $"https://api.travelpayouts.com/v1/city-directions?origin={origin}&currency={currency}&token={token}";

                var response = await _httpClient.GetAsync(apiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    System.Diagnostics.Debug.WriteLine("Attempting to deserialize JSON content.");
                    try
                    {
                        var result = JsonSerializer.Deserialize<PopularDirectionsData>(content, options);
                        System.Diagnostics.Debug.WriteLine("Deserialization complete.");
                        System.Diagnostics.Debug.WriteLine($"Data count: {result.Data.Keys.Count}");
                        return result;
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Deserialization failed: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Deserialization failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching data: {ex.Message}");
            }

            return null;
        }
    
        public async Task<ResponseData?> GetCalendarPricesData(string origin, string destination, string departDate, string returnDate, string currency, string token)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var apiUrl = $"https://api.travelpayouts.com/v1/prices/calendar?depart_date={departDate}&origin={origin}&return_date={returnDate}&destination={destination}&calendar_type=departure_date&currency={currency}&token={token}";

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                    try 
                    {
                        var result = JsonSerializer.Deserialize<ResponseData>(content, options);
                        return result;
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Deserialization failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching data: {ex.Message}");
            }

            return null;
        }
    }
}
    
public class PopularDirectionsData
{
    public Dictionary<string, DirectionDetails> Data { get; set; }
    public string Currency { get; set; }
    public bool Success { get; set; }
}

public class DirectionDetails
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Airline { get; set; }
    public string  departure_at { get; set; }
    public string  return_at { get; set; }
    
    public string  expires_at { get; set; }
    public int Price { get; set; }
    public int flight_number { get; set; }
    public int Transfers { get; set; }
}

public class ResponseData
{
    public Dictionary<string, DirectionDetails> Data { get; set; }
    public string Currency { get; set; }
    public bool Success { get; set; }
}