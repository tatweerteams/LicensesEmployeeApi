using Infra;
using LogginServices.Services;
using MediatR;
using SharedTatweerSendData.Events;

namespace LogginWebAPI.Queries
{
    public class GetLogginDataQuery : IRequest<ResultOperationDTO<PaginationDto<LogginDataEvent>>>
    {
        public string UserName { get; set; }
        public string BranchNo { get; set; }
        public string BranchName { get; set; }
        public EventTypeState? EventType { get; set; }
        public UserTypeState? UserType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

    }

    public class GetLogginDataHandler : IRequestHandler<GetLogginDataQuery, ResultOperationDTO<PaginationDto<LogginDataEvent>>>
    {

        private readonly IInsertLogginDataServices _getLogginData;
        public GetLogginDataHandler(IInsertLogginDataServices getLogginData)
        {
            _getLogginData = getLogginData;
        }
        public async Task<ResultOperationDTO<PaginationDto<LogginDataEvent>>> Handle(GetLogginDataQuery request, CancellationToken cancellationToken)
        {
            var result = await _getLogginData.
                GetLogginData(request.UserName, request.BranchNo, request.BranchName, request.EventType, request.UserType,
                request.From, request.To, request.PageNo, request.PageSize);

            return ResultOperationDTO<PaginationDto<LogginDataEvent>>.CreateSuccsessOperation(result);
        }
    }
}
