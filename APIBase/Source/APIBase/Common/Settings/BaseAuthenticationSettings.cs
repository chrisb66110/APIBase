using System.Diagnostics.CodeAnalysis;

namespace APIBase.Common.Settings
{
    [ExcludeFromCodeCoverage]
    public class BaseAuthenticationSettings
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
}
