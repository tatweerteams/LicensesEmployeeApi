using CollactionData.Models.AuthUserModel;
using IdentityAPI.Features.Commands.AuthUserCommands;
using IdentityAPI.Features.Queries;
using IdentityAPI.Filters.AuthLoginUserFilter;
using Infra;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters.AuthUserFilter;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityLoginController : BaseController
    {
        private readonly IMediator _mediator;
        private IAntiforgery _antiForgery;
        public SecurityLoginController(IMediator mediator, IAntiforgery antiForgery)
        {
            _mediator = mediator;
            _antiForgery = antiForgery;
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        [TypeFilter(typeof(CheckUserAccessFilter))]
        public async Task<ActionResult<ResultOperationDTO<UserAuthDTO>>> LoginUser([FromBody] LoginUserModel loginUser)
        {

            var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
            return await _mediator.Send(new LoginUserCommand
            {
                UserAuthSuccess = loginUser.UserAuth
            });
        }


        [Authorize]
        [HttpGet("GetUserInfo")]
        [TypeFilter(typeof(CheckUserIsAdminFilter))]
        public async Task<ActionResult<ResultOperationDTO<UserAuthDTO>>> GetUserInfo()
        => await _mediator.Send(new GetUserInfoQuery());

        [AllowAnonymous]
        [HttpPost("ChenagePassword")]
        public async Task<ActionResult<ResultOperationDTO<bool>>> ChangePassword([FromBody] ChengePasswordModel model)
            => await _mediator.Send(new ChengePasswordUserCommand
            {
                Model = model,
            });

        [HttpGet("GenerateAntiForgeryTokens")]
        public async Task<ResultOperationDTO<string>> GenerateAntiForgeryTokens()
        {
            var tokens = _antiForgery.GetAndStoreTokens(HttpContext);

            var result = _antiForgery.GetAndStoreTokens(HttpContext);
            //Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions
            //{
            //    HttpOnly = true
            //});
            return ResultOperationDTO<string>.CreateSuccsessOperation(tokens.RequestToken);
        }

    }
}
