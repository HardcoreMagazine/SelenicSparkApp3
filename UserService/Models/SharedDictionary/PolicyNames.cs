namespace UserService.Models.SharedDictionary
{
    /// <summary>
    /// Because enums can't be strings
    /// </summary>
    public static class PolicyNames
    {
        public const string AppUser = "User";
        public const string AppAdmin = "Admin";
        // the hardest part:
        public const string AppClaimOwner = "ClaimOwner";
    }
}
