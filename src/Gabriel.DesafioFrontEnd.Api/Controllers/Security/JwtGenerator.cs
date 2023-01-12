using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gabriel.DesafioFrontEnd.Api.Controllers.Security
{
    public class JwtGenerator
    {
        public JwtGenerator() { }

        public string GenerateTokenForClientCredentialFlow(
            string clientId,
            string clientSecret,
            string clientName,
            string[] scopes)
        {

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://identityserver.gabriel.com.br",
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, clientId),
                    new Claim(JwtRegisteredClaimNames.NameId, clientName),
                    new Claim(JwtRegisteredClaimNames.Sid, clientId)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = NewSigningCredentials(clientSecret)
            };

            var jwt = JwtHandler.CreateJwtSecurityToken(tokenDescriptor);
            jwt.Payload["scope"] = scopes;

            return JwtHandler.WriteToken(jwt);
        }

        public JwtSecurityToken Decode(string token) => JwtHandler.ReadJwtToken(token);

        private SigningCredentials NewSigningCredentials(string clientSecret)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(clientSecret));
            return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        }

        JwtSecurityTokenHandler JwtHandler => new JwtSecurityTokenHandler();

    }
}
