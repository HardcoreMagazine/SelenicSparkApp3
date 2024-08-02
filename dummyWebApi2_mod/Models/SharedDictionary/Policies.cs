namespace dummyWebApi2.Models.SharedDictionary
{
    /// <summary>
    /// Because enums can't be strings
    /// </summary>
    public static class Policies
    {
        public const string AppUser = "User";
        public const string AppAdmin = "Admin";
        // the hardest part:
        public const string AppContentOwner = "ContentOwner";
    }
}
