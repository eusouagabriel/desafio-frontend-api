using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Gabriel.DesafioFrontEnd.Api.Controllers.Security
{

    public static class AuthorizationHelper
    {
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, JwtSettings configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(new TokenValidator(configuration, new AuthorizedAppService()));
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.SigningCredentialsKey)),
                        ValidateAudience = false,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Token == null)
                            {
                                var headers = context.HttpContext.Request.Headers;
                                if (headers.ContainsKey("Authorization"))
                                {
                                    var jwt = headers["Authorization"].First();

                                    if (!jwt.StartsWith("Bearer"))
                                        context.Token = $"Bearer {jwt}";
                                    else
                                        context.Token = jwt;
                                }
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
    public class JwtSettings
    {
        public string SigningCredentialsKey { get; set; }
        public string Issuer { get; set; }
    }
}
