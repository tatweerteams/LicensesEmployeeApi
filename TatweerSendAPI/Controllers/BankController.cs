using Infra;
using Infra.Utili;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Models;
using TatweerSendAPI.Features.Commands.BankCommands;
using TatweerSendAPI.Features.Queries.BankQueries;
using TatweerSendAPI.Filters.BankFilter;

namespace TatweerSendAPI.Controllers
{
    [Route("api/[controller]")]

    public class BankController : BaseController
    {

        private readonly IMediator _mediator;
        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = RolesUtili.Administrator)]
        [HttpPost("InsertBank")]
        [TypeFilter(typeof(InsertBankFilter))]
        public async Task<ResultOperationDTO<bool>> InsertBank([FromBody] InsertBankModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new InsertBankCommand { BankModel = model });

        [Authorize(Roles = RolesUtili.Administrator)]
        [HttpPut("UpdateBank")]
        [TypeFilter(typeof(UpdateBankFilter))]
        public async Task<ResultOperationDTO<bool>> UpdateBank([FromBody] UpdateBankModel model, CancellationToken cancellationToken = default)
        => await _mediator.Send(new UpdateBankCommand { BankModel = model });

        [Authorize(Roles = RolesUtili.Administrator)]
        [HttpPut("ActivationBank")]
        [TypeFilter(typeof(ActivationBankFilter))]
        public async Task<ResultOperationDTO<bool>> ActivationBank(string bankId, bool isActive, CancellationToken cancellationToken = default)
        => await _mediator.Send(new ActivationBankCommand { BankId = bankId, IsActive = isActive });

        [Authorize(Roles = RolesUtili.Administrator)]
        [HttpGet("GetBanks")]
        public async Task<ResultOperationDTO<IReadOnlyList<BankDTO>>> GetBanks(string bankName, string bankNo, CancellationToken cancellationToken = default)
          => await _mediator.Send(new GetBankListQuery { BankName = bankName, BankNo = bankNo });

        [Authorize]
        [HttpGet("GetActiveBanks")]
        public async Task<ResultOperationDTO<IReadOnlyList<ActiveBankDTO>>> GetActiveBanks(string bankName, string bankNo, CancellationToken cancellationToken = default)
          => await _mediator.Send(new GetActivationBankListQuery { BankName = bankName, BankNo = bankNo });

        [Authorize(Roles = RolesUtili.Administrator)]
        [HttpDelete("DeleteBank")]
        [TypeFilter(typeof(DeleteBankFilter))]
        public async Task<ResultOperationDTO<bool>> DeleteBank(string bankId, CancellationToken cancellationToken = default)
        => await _mediator.Send(new DeleteBankCommand { BankId = bankId });


    }
}
