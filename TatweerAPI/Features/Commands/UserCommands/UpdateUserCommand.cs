using CollactionData.Models.Users;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.UserCommands
{
    public class UpdateUserCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateUserModel Model { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ResultOperationDTO<bool>>
    {

        private readonly IUserServices _userServices;
        public UpdateUserHandler(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<ResultOperationDTO<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userServices.UpdateUser(request.Model);
            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم تعديل بيانات المستخدم" });
        }
    }
}
