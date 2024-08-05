using Microsoft.AspNetCore.Authorization;

namespace dummyWebApi2.Services.Security
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public string RequirementName { get; }
        public string? RequirementRole { get; } = null;

        public AuthorizationRequirement(string name)
        {
            RequirementName = name;
        }

        public AuthorizationRequirement(string name, string role)
        {
            RequirementName = name;
            RequirementRole = role;
        }
    }
}
