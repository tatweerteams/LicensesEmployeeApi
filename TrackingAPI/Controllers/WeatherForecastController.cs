using Microsoft.AspNetCore.Mvc;
using Services;

namespace ReciveAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITrackServices _trackServices;
        public WeatherForecastController(ITrackServices trackServices)
        {
            _trackServices = trackServices;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<bool> Get()
        {
            await _trackServices.GetOrderRequestIsPrinting();
            await _trackServices.GetOrderRequestIsDone();
            return true;
        }
    }
}