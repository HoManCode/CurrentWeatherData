namespace BackEnd.Controllers;


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private static Dictionary<string, int> ApiKeyUsage = new();
    private readonly IConfiguration _configuration;

    public WeatherController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeather([FromQuery] string city, [FromQuery] string country)
    {
        if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            return BadRequest(new { message = "City and Country are required." });

        var apiKeys = _configuration.GetSection("OpenWeatherMap:ApiKeys").Get<string[]>();
        string apiKey = apiKeys.FirstOrDefault(k => !ApiKeyUsage.ContainsKey(k) || ApiKeyUsage[k] < 5);

        if (string.IsNullOrEmpty(apiKey))
        {
            return StatusCode(429, new { message = "Hourly limit for all API keys has been exceeded." });
        }

        ApiKeyUsage.TryGetValue(apiKey, out int usage);

        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid={apiKey}";

        try
        {
            using var client = new HttpClient();
            var response = await client.GetFromJsonAsync<dynamic>(url);

            if (response == null || response.weather == null)
                return NotFound(new { message = "Weather data not found." });

            var description = response.weather[0].description.ToString();

            // Increment API usage count
            ApiKeyUsage[apiKey] = usage + 1;

            return Ok(new { description });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
        }
    }
}