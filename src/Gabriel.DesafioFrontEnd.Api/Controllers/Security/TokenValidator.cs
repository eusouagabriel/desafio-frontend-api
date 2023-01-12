using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gabriel.DesafioFrontEnd.Api.Controllers.Security
{
    public class TokenValidator : ISecurityTokenValidator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly AuthorizedAppService _appService;

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CanReadToken(string securityToken) => !string.IsNullOrWhiteSpace(securityToken);

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            return ValidateToken(securityToken, validationParameters, out validatedToken, _jwtSettings.SigningCredentialsKey);
        }

        protected ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken, string credentialKey)
        {
            var securityTokenValue = securityToken.Replace("Bearer", string.Empty).Trim();
            var jwtHandler = new JwtSecurityTokenHandler();

            var rawToken = new JwtSecurityToken(securityTokenValue);

            var issuer = _jwtSettings.Issuer;
            if (rawToken.Issuer == issuer)
            {
                var appId = rawToken.Claims.SingleOrDefault(x => x.Type.Equals("sid")).Value;
                var authorizedApp = _appService.GetAppById(Guid.Parse(appId));
                validationParameters.ValidIssuer = issuer;
                validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizedApp.ClientSecret));
            }

            return jwtHandler.ValidateToken(securityTokenValue, validationParameters, out validatedToken);
        }

        public TokenValidator(
            JwtSettings jwtSettings,
            AuthorizedAppService appService)
        {
            _jwtSettings = jwtSettings;
            _appService = appService;
        }

    }
}
