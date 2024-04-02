using Microsoft.AspNetCore.Mvc;

namespace Servicio2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


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
        [Route("servicio1")]
        public async Task<string> Servicio1()
        {
            await Servicio2InvocaServicio1();
            return "Servicio2 invoca a Servicio1";
        }

        private async Task Servicio2InvocaServicio1()
        {
            var httpClient = _httpClientFactory.CreateClient();

            foreach (var header in Request.Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
            }

            _ = await httpClient.GetStringAsync("http://localhost:5170/WeatherForecast");
        }
    }
}
