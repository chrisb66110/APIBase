using System.Linq;
using APIBase.Common.Constants;
using APIBase.Common.RsaSecurityKeyReader;
using APIBase.Common.Settings;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace APIBase.Api.Configurations
{
    public static class Configure
    {
        public static void ConfigureAuthentication<TSettings>(
            this IServiceCollection services,
            IContainer container,
            string tempKeyPath)
            where TSettings : BaseSettings
        {
            TSettings settings;

            using (var scope = container.BeginLifetimeScope())
            {
                settings = scope.Resolve<TSettings>();
            }

            var signInKey = RsaSecurityKeyReader.GetSignInKey(tempKeyPath);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddCookie("dummy")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.AuthenticationSettings.Authority,
                    ValidAudience = settings.AuthenticationSettings.Audience,
                    IssuerSigningKey = signInKey
                };
            });
        }

        public static void ConfigureCors<TSettings>(
            this IServiceCollection services,
            IContainer container,
            params string[] methodsAllowed)
            where TSettings : BaseSettings
        {
            TSettings settings;

            using (var scope = container.BeginLifetimeScope())
            {
                settings = scope.Resolve<TSettings>();
            }

            var allowedCors = settings.AllowedOrigins.Values.Select(cor => cor).ToArray();

            services.AddCors(options =>
            {
                options.AddPolicy(BaseConstants.ALLOWED_CORS_POLICY, builder =>
                {
                    builder.WithOrigins(allowedCors);
                    builder.WithMethods(methodsAllowed);
                });
            });
        }
    }
}
