using AutoMapper;
using CollactionData.DTOs;
using CollactionData.Models.PermisstionModel;
using Domain.Domain;
using FilterAttributeWebAPI.Common;
using Infra;

namespace IdentityServices.services
{
    public interface IPermisstionServices
    {
        Task<IReadOnlyList<ActivePermisstionDTO>> GetActivePermisstions(string name);
        Task ActivationPermisstion(string id, bool isActive);
        Task<PaginationDto<PermisstionDTO>> GetPermisstions(string name, int page = 1, int pageSize = 30);
        Task InsertPermisstion(InsertPermisstionModel model);
        Task UpdatePermisstion(UpdatePermisstionModel model);

    }

    public class PermisstionServices : IPermisstionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PermisstionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task ActivationPermisstion(string id, bool isActive)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Permisstion>().GetByID(id);

            if (result == null)
                throw new ApplicationEx("بيانات الصلاحية غير موجودة");

            result.IsActive = !isActive;

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IReadOnlyList<ActivePermisstionDTO>> GetActivePermisstions(string name)
        => await _unitOfWork.GetRepositoryReadOnly<Permisstion>().FindBy(
                    predicate: pred =>
                    pred.IsActive == true &&
                    (string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name)),

                    selector: select => new ActivePermisstionDTO
                    {
                        Description = select.Description,
                        Id = select.Id
                    },
                    pageNo: 1,
                    pageSize: 40
                );

        public async Task<PaginationDto<PermisstionDTO>> GetPermisstions(string name, int page = 1, int pageSize = 30)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Permisstion>().FindBy(
                    predicate: pred => (string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name)),
                    selector: select => new PermisstionDTO
                    {
                        Name = select.Name,
                        Description = select.Description,
                        Id = select.Id,
                        IsActive = select.IsActive
                    }
                );

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<Permisstion>().
                   GetCount(pred => (string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name)));

            return new PaginationDto<PermisstionDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };
            throw new NotImplementedException();
        }

        public async Task InsertPermisstion(InsertPermisstionModel model)
        {
            await _unitOfWork.GetRepositoryWriteOnly<Permisstion>().Insert(_mapper.Map<Permisstion>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdatePermisstion(UpdatePermisstionModel model)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<Permisstion>().GetByID(model.Id);

            if (oldData == null)
                throw new ApplicationEx(" بيانات الصلاحية غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<Permisstion>().Update(_mapper.Map(model, oldData));
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
