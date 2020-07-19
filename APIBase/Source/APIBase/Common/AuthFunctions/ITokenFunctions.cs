using System.Collections.Generic;

namespace APIBase.Common.AuthFunctions
{
    public interface ITokenFunctions
    {
        string GetUsername();
        string GetEmail();
        List<string> GetRoles();
        List<string> GetScopes();
    }
}
