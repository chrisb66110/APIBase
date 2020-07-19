using System.Collections.Generic;
using System.Linq;
using APIBase.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace APIBase.Common.AuthFunctions
{
    public class TokenFunctions : ITokenFunctions
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenFunctions(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(BaseConstants.AUTH_CLAIM_USERNAME)?.Value;

            return username;
        }

        public string GetEmail()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(BaseConstants.AUTH_CLAIM_EMAIL)?.Value;

            return email;
        }

        public List<string> GetRoles()
        {
            var roles = _httpContextAccessor.HttpContext.User.FindAll(BaseConstants.AUTH_CLAIM_ROLE).Select(r => r.Value).ToList();

            return roles;
        }

        public List<string> GetScopes()
        {
            var scope = _httpContextAccessor.HttpContext.User.FindAll(BaseConstants.AUTH_CLAIM_SCOPE).Select(r => r.Value).ToList();

            return scope;
        }
    }
}
