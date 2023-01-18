using Gabriel.DesafioFrontEnd.Api.Controllers.Resources;
using Gabriel.DesafioFrontEnd.Api.Controllers.Security;
using Microsoft.AspNetCore.Mvc;

namespace Gabriel.DesafioFrontEnd.Api.Controllers
{

    [Route("security")]
    [ApiController]
    [Produces("application/json")]
    public class SecurityController : ControllerBase
    {
        /// <summary>
        /// Obtem o bearer token para autenticação e autorização na api.
        /// </summary>
        /// <param name="clientId"> A chave do app autorizado a utilizar os recursos nesta api </param>
        /// <param name="clientSecret">O segredo gerado para o app autorizado a utilizar os recursos da api</param>
        /// <returns> retorna o token para ser utilizado nos recursos disponíveis na api</returns>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     GET /token?clientId=01878fb6-4206-40e0-b195-46f465a3a65b&amp;clientSecret=38c8a848d23e34f281b6dfcd35b11d79f0dbdbd0372269b38f46b6a5e9fe6006  
        ///     
        ///     Para este desafio o app registrado possui os seguintes parametros:
        ///     
        ///     clientId = 01878fb6-4206-40e0-b195-46f465a3a65b
        ///     clientSecret = 38c8a848d23e34f281b6dfcd35b11d79f0dbdbd0372269b38f46b6a5e9fe6006
        ///     
        /// </remarks>
        /// <response code="200">
        /// Retorna o tipo do token e o token de acesso
        /// </response>
        /// <response code="401">
        /// Quando o app registrado não for encontrado
        /// </response>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("/token")]
        public async Task<IActionResult> GetToken([FromQuery]string clientId, [FromQuery]string clientSecret, AuthorizedAppService appService)
        {

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret)) return Unautorized();

            var app = appService.GetAppBy(Guid.Parse(clientId), clientSecret);

            if (app == null) return Unautorized();

            return Ok(new
            {
                token_type = "Bearer",
                access_token = new JwtGenerator().GenerateTokenForClientCredentialFlow(clientId, clientSecret, app.Name, app.Scopes),
            });

          IActionResult Unautorized() => Unauthorized(
          new
          {
              error = "invalid_request",
              error_description = "Não authorizado.",
          });

           
        }



    }
}
