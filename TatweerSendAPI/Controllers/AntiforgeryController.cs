using Infra;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace TatweerSendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AntiforgeryController : ControllerBase
    {
        private IAntiforgery _antiForgery;
        public AntiforgeryController(IAntiforgery antiforgery)
        {
            _antiForgery = antiforgery;
        }

        [HttpGet("GenerateAntiForgeryTokenTatweerSend")]
        public async Task<ResultOperationDTO<string>> GenerateAntiForgeryTokenTatweerSend()
        {
            var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions
            {
                HttpOnly = false
            });
            return ResultOperationDTO<string>.CreateSuccsessOperation(tokens.RequestToken);
        }

    }
}
