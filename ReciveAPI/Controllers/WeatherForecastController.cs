using Microsoft.AspNetCore.Mvc;

namespace ReciveAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<bool> Get()
        {

            return true;
        }
    }
}