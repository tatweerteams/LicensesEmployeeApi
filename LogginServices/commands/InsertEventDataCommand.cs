using Infra;
using LogginServices.Services;
using MediatR;
using SharedTatweerSendData.Events;

namespace LogginServices.commands
{
    public class InsertEventDataCommand : IRequest<ResultOperationDTO<bool>>
    {
        public LogginDataEvent DataEvent { get; set; }
    }

    public class InsertEventDataHandler : IRequestHandler<InsertEventDataCommand, ResultOperationDTO<bool>>
    {
        private readonly IInsertLogginDataServices _insertLogginData;
        public InsertEventDataHandler(IInsertLogginDataServices insertLogginData)
        {
            _insertLogginData = insertLogginData;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertEventDataCommand request, CancellationToken cancellationToken)
        {
            await _insertLogginData.InsertLogginEvent(request.DataEvent);

            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(true, message: new string[] { "تمت عملية بنجاح" });

        }
    }
}
