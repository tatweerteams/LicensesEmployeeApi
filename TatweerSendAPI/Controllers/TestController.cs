using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TatweerSendAPI.Controllers
{
    [Route("api/[controller]")]
    public class TestController : BaseController
    {
        private readonly HelperUtili _helper;
        public TestController(HelperUtili helper)
        {
            _helper = helper;
        }

        //[HttpPost("test")]
        ////[TypeFilter(typeof(ResetPasswordFilter))]
        //public async Task<ActionResult<ResultOperationDTO<bool>>> test()
        //{
        //    var result = _helper.GetCurrentUser();

        //    return ResultOperationDTO<bool>.CreateSuccsessOperation(true);
        //}
    }
}
