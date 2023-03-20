using AutoMapper;
using Infra;
using LogginDomain;
using LogginServices.Services.ExtensionServices;
using Microsoft.EntityFrameworkCore;
using SharedTatweerSendData.Events;

namespace LogginServices.Services
{
    public interface IInsertLogginDataServices
    {
        Task InsertLogginEvent(LogginDataEvent dataEvent);
        Task<PaginationDto<LogginDataEvent>> GetLogginData(string userName, string branchNo, string branchName,
            EventTypeState? eventType, UserTypeState? userType, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30);
    }

    public class InsertLogginDataServices : IInsertLogginDataServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InsertLogginDataServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationDto<LogginDataEvent>> GetLogginData(string userName, string branchNo, string branchName, EventTypeState? eventType,
            UserTypeState? userType, DateTime? from, DateTime? to, int pageNo = 1, int pageSize = 30)
        {
            var filterData = await _unitOfWork.GetRepositoryReadOnly<LogginData>().
                FindBy(userName.SearchLogginEventExpression(branchNo, branchName, eventType, userType, from, to));

            var totalRecordCount = await filterData.CountAsync();

            var result = await filterData.Select(select => new LogginDataEvent
            {
                EventType = select.EventType,
                BranchNumber = select.BranchNumber,
                CreateAt = select.CreateAt,
                Messages = select.Messages,
                NewData = select.NewData,
                OldData = select.OldData,
                UserId = select.UserId,
                UserName = select.UserName,
                UserType = select.UserType
            }).Skip((pageNo - 1) * pageSize).
           Take(pageSize).
           OrderByDescending(o => o.CreateAt).
           ToListAsync();

            return new PaginationDto<LogginDataEvent>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };

        }

        public async Task InsertLogginEvent(LogginDataEvent dataEvent)
        {
            var result = _mapper.Map<LogginData>(dataEvent);

            await _unitOfWork.GetRepositoryWriteOnly<LogginData>().Insert(result);

            await _unitOfWork.SaveChangeAsync();
        }
    }
}
