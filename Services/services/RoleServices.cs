using AutoMapper;
using CollactionData.DTOs;
using CollactionData.Models.RoleModel;
using Domain.Domain;
using FilterAttributeWebAPI.Common;
using IdentityServices.services.ExtenstionServices;
using Infra;

namespace IdentityServices.services
{
    public interface IRoleServices
    {
        Task InsertRole(InsertRoleModel model);
        Task UpdateRole(UpdateRoleModel model);
        Task ActivationRole(string id, bool isActive);
        Task DeleteRole(string id);
        Task<PaginationDto<RoleDTO>> GetRoles(string name, UserTypeState? userType, int pageNo = 1, int pageSize = 30);
        Task<IReadOnlyList<ActiveRoleDTO>> GetActiveRoles(UserTypeState userType, string name);
    }

    public class RoleServices : IRoleServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoleServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task ActivationRole(string id, bool isActive)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Role>().GetByID(id);

            if (result == null)
                throw new ApplicationEx("بيانات الصلاحية غير موجودة");

            result.IsActive = !isActive;

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteRole(string id)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Role>().GetByID(id);

            if (result == null)
                throw new ApplicationEx("بيانات الصلاحية غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<Role>().Remove(result);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IReadOnlyList<ActiveRoleDTO>> GetActiveRoles(UserTypeState userType, string name)
            => await _unitOfWork.GetRepositoryReadOnly<Role>().FindBy(
                predicate: pred =>
                        pred.IsActive == true &&
                        pred.UserType.Equals(userType) &&
                        (string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name)),
                selector: select => new ActiveRoleDTO
                {
                    Id = select.Id,
                    Name = select.Name,
                },
                pageNo: 1,
                pageSize: 20);

        public async Task<PaginationDto<RoleDTO>> GetRoles(string name, UserTypeState? userType, int pageNo = 1, int pageSize = 30)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Role>().FindBy(
                predicate: name.SearchRoleExpression(userType),
                selector: select => new RoleDTO
                {
                    CreateAt = select.CreateAt,
                    Id = select.Id,
                    IsActive = select.IsActive,
                    Name = select.Name,
                    UserName = select.CreateUser.Name,
                    UserType = select.UserType,
                    RolePermisstions = select.RolePermisstions.Select(selectPermisstion => new ActivePermisstionDTO
                    {
                        Description = selectPermisstion.Permisstion.Description,
                        Id = selectPermisstion.PermisstionId
                    }).ToList()
                },
                pageNo: pageNo,
                pageSize: pageSize);

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<Role>().
                    GetCount(name.SearchRoleExpression(userType));

            return new PaginationDto<RoleDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };

        }

        public async Task InsertRole(InsertRoleModel model)
        {
            await _unitOfWork.GetRepositoryWriteOnly<Role>().Insert(_mapper.Map<Role>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateRole(UpdateRoleModel model)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<Role>().SingalOfDefultWithIncludAsync(
                    predicate: pred => pred.Id == model.Id,
                    include: include => include.RolePermisstions
                );

            if (oldData == null)
                throw new ApplicationEx(" بيانات الدور غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<Role>().Update(_mapper.Map(model, oldData));
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
