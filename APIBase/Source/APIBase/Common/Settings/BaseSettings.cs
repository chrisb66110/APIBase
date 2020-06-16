using System.Diagnostics.CodeAnalysis;

namespace APIBase.Common.Settings
{
    [ExcludeFromCodeCoverage]
    public class BaseSettings
    {
        public BaseAuthenticationSettings AuthenticationSettings { get; set; }
    }
}
