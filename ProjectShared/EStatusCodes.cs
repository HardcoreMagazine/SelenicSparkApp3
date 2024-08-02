namespace SharedLibCS
{
    /// <summary>
    /// When updating this ENUM - remember to update same ENUM in 'frontend' project: 
    /// '...\SelenicSparkApp_v3\Frontend\src\components\Shared\Scriprs\EStatusCodes.ts'
    /// </summary>
    public enum StatusCodes
    {
        Ok = 0,
        ClientFail = -1,
        ServerFail = -2,
        ServerTimeout = -3,
        BadCredentials = -4,
    }
}
