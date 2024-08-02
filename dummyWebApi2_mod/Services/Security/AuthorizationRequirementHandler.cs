using dummyWebApi2.Models.SharedDictionary;
using Microsoft.AspNetCore.Authorization;

namespace dummyWebApi2.Services.Security
{
    public class AuthorizationRequirementHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthorizationRequirementHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            var httpRequest = _contextAccessor.HttpContext?.Request;
            if (httpRequest == null)
            {
                context.Fail();
            }
            else
            {
                var header = httpRequest.Headers[requirement.RequirementName];
                if (header.Count > 0)
                {
                    var jwtToken = header
                        .ToString()
                        .Replace($"{PolicySettingNames.BearerScheme} ", "");

                    bool validationResult;
                    if (requirement.RequirementRole != null)
                    {
                        validationResult = TokenManager.ValidateJwtTokenWithRole(jwtToken, requirement.RequirementRole);
                    }
                    else
                    {
                        validationResult = TokenManager.ValidateJwtToken(jwtToken);
                    }

                    if (validationResult)
                        context.Succeed(requirement);
                    else
                        context.Fail();
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }
    }
}
