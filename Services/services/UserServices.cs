using AutoMapper;
using CollactionData.DTOs;
using CollactionData.Models.Users;
using Domain;
using FilterAttributeWebAPI.Common;
using IdentityServices.services.ExtenstionServices;
using Infra;
using Infra.Utili;

namespace IdentityServices.services
{
    public interface IUserServices
    {
        Task InsertUser(InsertUserModel model);
        Task UpdateUser(UpdateUserModel model);
        Task ActivationUser(string id, bool isActive);
        Task<PaginationDto<UserDTO>> GetUsers(string branchId, UserTypeState? userType, string name, string regionId,
                string employeeNo, string phoneNumber, int pageNo, int pageSize);
        Task ResetPassword(string userId, string newPassword, bool sendPassword);

    }

    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HelperUtili _helper;
        private readonly IMapper _mapper;
        public UserServices(IUnitOfWork unitOfWork, HelperUtili helper, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _helper = helper;
            _mapper = mapper;
        }
        public async Task ActivationUser(string id, bool isActive)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<User>().GetByID(id);
            if (result == null)
                throw new ApplicationEx("بيانات المستخدم غير موجودة");

            result.IsActive = !isActive;

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<PaginationDto<UserDTO>> GetUsers(string branchId, UserTypeState? userType, string name, string regionId,
            string employeeNo, string phoneNumber, int pageNo, int pageSize)
        {
            var currentUser = _helper.GetCurrentUser();
            branchId = branchId ?? currentUser?.BranchId;
            regionId = regionId ?? currentUser?.RegionId;

            var result = await _unitOfWork.GetRepositoryReadOnly<User>().FindBy(
                    predicate: branchId.SearchUserExpression(userType, name, regionId, employeeNo, phoneNumber),
                    selector: select => new UserDTO
                    {
                        Id = select.Id,
                        Name = select.Name,
                        UserType = select.Role.UserType,
                        BankId = select.BankId,
                        BankName = select.BankName,
                        BranchId = select.BranchId,
                        BranchName = select.BranchName,
                        BranchNumber = select.BranchNumber,
                        CreateAt = select.CreateAt,
                        Email = select.Email,
                        EmployeeNumber = select.EmployeeNumber,
                        IsActive = select.IsActive,
                        PhoneNumber = select.PhoneNumber,
                        RegionId = select.RegionId,
                        RegionName = select.RegionName,
                        RoleId = select.RoleId,
                        RoleName = select.Role.Name,
                    },
                    pageNo: pageNo,
                    pageSize: pageSize);

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<User>().
                   GetCount(branchId.SearchUserExpression(userType, name, regionId, employeeNo, phoneNumber));

            return new PaginationDto<UserDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };
        }

        public async Task InsertUser(InsertUserModel model)
        {
            var mappUser = _mappUserExtenstion(model);

            await _unitOfWork.GetRepositoryWriteOnly<User>().Insert(_mapper.Map<User>(mappUser));
            await _unitOfWork.SaveChangeAsync();

            //if(mappUser.SendPassword && (!string.IsNullOrWhiteSpace(mappUser.Email) || !string.IsNullOrWhiteSpace(mappUser.PhoneNumber)))
            //    sendToEventSMSAndEmail 
        }

        public async Task ResetPassword(string userId, string newPassword, bool sendPassword)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<User>().GetByID(userId);
            if (oldData == null)
                throw new ApplicationEx("بيانات المستخدم غير موجودة");

            oldData.PasswordHash = _helper.Hash(newPassword);
            oldData.IsFirstLogin = false;
            await _unitOfWork.SaveChangeAsync();
            //if(mappUser.SendPassword && (!string.IsNullOrWhiteSpace(oldData.Email) || !string.IsNullOrWhiteSpace(oldData.PhoneNumber)))
            // send Event SMS

        }

        public async Task UpdateUser(UpdateUserModel model)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<User>().GetByID(model.Id);
            if (oldData == null)
                throw new ApplicationEx("بيانات المستخدم غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<User>().Update(_mapper.Map(model, oldData));
            await _unitOfWork.SaveChangeAsync();
        }

        private InsertUserModel _mappUserExtenstion(InsertUserModel model)
        {
            var currentUser = _helper.GetCurrentUser();
            model.BranchId = model.BranchId ?? currentUser.BranchId;
            model.BranchName = model.BranchName ?? currentUser.BranchName;
            model.BranchNumber = model.BranchNumber ?? currentUser.BranchNumber;

            model.RegionId = model.RegionId ?? currentUser.RegionId;
            model.RegionName = model.RegionName ?? currentUser.RegionName;

            model.BankId = model.BankId ?? currentUser.BankId;
            model.BankName = model.BankName ?? currentUser.BankName;

            model.PasswordHash = _helper.Hash(model.PasswordHash);

            return model;
        }
    }
}
