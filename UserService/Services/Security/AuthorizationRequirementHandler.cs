using Microsoft.AspNetCore.Authorization;
using UserService.Models.InAppDictionary;
using UserService.Services.Security;

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
            if (httpRequest == null) // very rare and likely unrealistic scenario, but still - better be on the safe side
            {
                context.Fail();
            }
            else
            {
                var header = httpRequest.Headers[requirement.RequirementName];
                if (header.Count > 0)
                {
                    var token = header
                        .ToString()
                        .Replace($"{PolicySettingNames.CurrentScheme} ", "");

                    var validationResult = JwtTokenManager.ValidateUserToken(token);
                    
                    if (!validationResult)
                    {
                        context.Fail();
                    }
                    else
                    {
                        if (requirement.RequirementRole != null)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            context.Succeed(requirement);
                        }
                    }
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
