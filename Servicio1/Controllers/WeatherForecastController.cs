using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace CorrelationId1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            //_logger.LogInformation("Log personalizado");
            _logger.LogWarning("Log personalizado");
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException ex)
            {
                _logger.LogError(ex, ex.Message);
            }


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet]
        [Route("servicio2")]
        public async Task<string> Servicio2()
        {
            await Servicio1InvocaServicio2();
            return "Servicio1 invoca a Servicio2";
        }

        private async Task Servicio1InvocaServicio2()
        {
            var httpClient = _httpClientFactory.CreateClient();

            foreach (var header in Request.Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
            }

            _ = await httpClient.GetStringAsync("http://localhost:39270/WeatherForecast");

        }
    }

}
