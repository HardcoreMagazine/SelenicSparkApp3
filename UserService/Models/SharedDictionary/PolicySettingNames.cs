namespace UserService.Models.InAppDictionary
{
    /// <summary>
    /// Because ENUMs cannot be strings
    /// </summary>
    public static class PolicySettingNames
    {
        public const string AuthorizedRequirement = "Authorization";
        // be careful when changing current scheme, it has direct impact on AuthorizationRequirementHandler logic
        public const string CurrentScheme = "Bearer";
    }
}
