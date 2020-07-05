using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace APIBase.Common.Settings
{
    [ExcludeFromCodeCoverage]
    public class BaseSettings
    {
        public BaseAuthenticationSettings AuthenticationSettings { get; set; }

        public Dictionary<string, string> AllowedOrigins { get; set; }
    }
}
