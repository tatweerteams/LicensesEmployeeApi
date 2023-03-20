using CollactionData.Models.Users;
using IdentityServices.services;
using Infra;
using MediatR;

namespace IdentityAPI.Features.Commands.UserCommands
{
    public class InsertUserCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertUserModel Model { get; set; }
    }

    public class InsertUserHandler : IRequestHandler<InsertUserCommand, ResultOperationDTO<bool>>
    {
        private readonly IUserServices _userServices;

        public InsertUserHandler(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertUserCommand request, CancellationToken cancellationToken)
        {
            await _userServices.InsertUser(request.Model);

            return ResultOperationDTO<bool>.CreateSuccsessOperation(true, new string[] { "تم إضافة المستخدم " });
        }
    }

}
