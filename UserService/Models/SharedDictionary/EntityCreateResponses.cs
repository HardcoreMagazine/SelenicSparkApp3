namespace UserService.Models.SharedDictionary
{
    public enum EntityCreateResponses
    {
        Success = 0,
        EmailInUse = -1,
        UsernameInUse = -2,
        Exists = -3, // similar to EmailInUse/UsernameInUse, but more generalized
    }
}
