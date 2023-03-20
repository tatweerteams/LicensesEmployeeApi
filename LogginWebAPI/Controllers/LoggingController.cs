using Infra;
using Infra.Utili;
using LogginWebAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.Events;

namespace LogginWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoggingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoggingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = RolesUtili.Administrator)]
        [HttpGet("GetLogginData")]
        public async Task<ResultOperationDTO<PaginationDto<LogginDataEvent>>> GetLogginData(string userName, string branchNo, string branchName,
            EventTypeState? eventType, UserTypeState? userType, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
        => await _mediator.Send(new GetLogginDataQuery
        {

            To = to,
            UserName = userName,
            BranchNo = branchNo,
            BranchName = branchName,
            From = from,
            UserType = userType,
            EventType = eventType,
            PageNo = pageNo,
            PageSize = pageSize
        });
    }
}
