using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TatweerSendAPI.Filters;

namespace TatweerSendAPI.Controllers
{

    [TypeFilter(typeof(CheckBranchWorkTimeFilter))]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {

    }
}
