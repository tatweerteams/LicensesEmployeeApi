using CollactionData.DTOs;
using Infra;
using Infra.Utili;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace WebAPI.Filters.AuthUserFilter
{
    public class CheckUserIsAdminFilter : ActionFilterAttribute
    {
        private readonly HelperUtili _helper;
        private readonly IOptions<UserSystemDTO> _userSystem;

        public CheckUserIsAdminFilter(HelperUtili helper,
            IOptions<UserSystemDTO> userSystem)
        {
            _helper = helper;
            _userSystem = userSystem;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = _helper.GetCurrentUser();
            if (currentUser != null)
            {
                var adminSystem = _userSystem.Value;

                if (currentUser.UserName.Equals(adminSystem.Name))
                {
                    var userAuth = new UserAuthDTO()
                    {
                        Name = adminSystem.Name,
                        Permisstions = new List<string>() { adminSystem.Permisstion },
                        RoleName = adminSystem.Name,
                        Id = adminSystem.Name,
                        RoleId = "",
                        UserTypeState = UserTypeState.SuperAdmin,
                    };
                    context.Result = new OkObjectResult(
                        ResultOperationDTO<UserAuthDTO>.CreateSuccsessOperation(userAuth));
                    return;
                }
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
