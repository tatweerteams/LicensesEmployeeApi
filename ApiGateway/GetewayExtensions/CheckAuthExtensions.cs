using Infra;
using Newtonsoft.Json;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.GetewayExtensions
{
    public class CheckAuthExtensions
    {

        public async static Task<bool> CheckAuthenticated(HttpContext context)
        {
            await Task.FromResult(true);
            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var downstreamRoute = context.Items.DownstreamRoute();

            if (downstreamRoute == null)
                return false;

            if (downstreamRoute?.IsAuthenticated == true && string.IsNullOrWhiteSpace(token))
                return false;

            if (downstreamRoute?.IsAuthenticated == true && !IsValideToken(token))
                return false;

            return true;

        }

        public async static Task<bool> CheckIsAuthorization(HttpContext context)
        {
            await Task.FromResult(true);
            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var downstreamRoute = context.Items.DownstreamRoute();


            if (downstreamRoute.IsAuthorized == true)
            {
                var userClims = ExtractClaims(token).Where(w => w.Type == "role").Select(s => s?.Value).ToList();
                var claimRole = downstreamRoute.RouteClaimsRequirement.Values.ToList();


                return userClims.Any(a => claimRole.Contains(a));
            }

            return true;

        }

        private static bool IsValideToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.CanReadToken(token);

        }

        private static IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);


            IEnumerable<Claim> claims = securityToken.Claims;
            return claims;
        }

        public static StringContent GetStringResponse(string messages)
        {
            var result = new
            {
                Messages = messages,
            };
            var resultOperation = ResultOperationDTO<string>.CreateErrorOperation(messages: new string[] {
                result.Messages
            }, stateResult: StateResult.UnAuth);

            var jsonString = JsonConvert.SerializeObject(resultOperation, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return content;
        }

    }
}
