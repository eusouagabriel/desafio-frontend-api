using AutoMapper;
using Gabriel.DesafioFrontEnd.Api.Controllers.Resources;
using Gabriel.DesafioFrontEnd.Api.Domain.Service;
using Gabriel.DesafioFrontEnd.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Gabriel.DesafioFrontEnd.Api.Controllers
{

    /// <summary>
    /// Fornece os principais verbos para operar os recursos da entidade "cliente"
    /// </summary>
    [Route("customers")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class CustomersController : ControllerBase
    {

        /// <summary>Obtem o total e a coleção de clientes limitada pelo tamanho de sua página.</summary>
        /// <param name="filter">
        /// [searchCritéria] - Representa o critério de busca na entidade clientes.
        /// [pageNumber] - Obrigatório - representa a página que você deseja visualizar
        /// [pageSize] - Obrigatório - Representa o tamanho da página que você deseja visualizar.
        /// </param>
        /// <returns>
        /// Retorna o total de registro e a coleção de clientes.
        /// Veja o schema: CustomerResponse
        /// </returns>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     GET /customers?pagenumber=0&amp;pageSize=10  
        ///         [retornará os 10 registros da primeira página.]
        ///         
        ///     GET /customers?searchCriteria=rua&amp;pagenumber=0&amp;pageSize=10
        ///         [retornará a coleção de clientes com cujo o nome ou endereço contenham este critério de pesquisa limitada pelo tamnaho da página.]
        ///     
        /// </remarks>
        /// <response code="200">
        /// Retorna o total de registro e coleção de clientes.
        /// </response>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<CustomerResponse>> GetCustomer([FromQuery] CustomerGetFilter filter)
        {

            var result =  await _service.GetCustomers(filter.SearchCriteria, filter.PageNumber, filter.PageSize);

            return Ok(new CustomerResponse
            {
                TotalRows = result.TotalRow,
                Customers = _mapper.Map<IEnumerable<CustomerGetResponse>>(result.Customers)
            });
        }

        /// <summary>
        /// Obtem um cliente
        /// </summary>
        /// <param name="id">identificador unico do cliente</param>
        /// <returns>
        /// O Cliente
        /// Veja o schema: CustomerGetResponse
        /// </returns>
        /// <remarks>
        /// Exemplo: 
        /// 
        ///     GET /customers/{id} 
        ///     
        /// </remarks>
        /// <response code="200">
        /// Retorna o cliente.
        /// </response>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerGetResponse>> GetCustomer(Guid id) 
            => Ok(_mapper.Map<CustomerGetResponse>(await _service.GetCustomer(id)));

        /// <summary>
        /// Obtem o cliente e sua cameras
        /// </summary>
        /// <param name="id">o identificador unico do cliente</param>
        /// <returns>
        /// cliente com suas cameras
        /// Veja o schema: CustomerCamerasGetResponse
        /// </returns>
        /// <remarks>
        /// Exemplo:
        /// 
        ///     GET /customers/{id}/cameras
        /// </remarks>
        /// <response code="200">
        /// Retorna o cliente com suas cameras.
        /// </response>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}/cameras")]
        public async Task<ActionResult<CustomerCamerasGetResponse>> Get(Guid id)
            => Ok(_mapper.Map<CustomerCamerasGetResponse>(await _service.GetCustomer(id)));


        /// <summary>
        /// Executa somente a operação de atualização de um cliente
        /// </summary>
        /// <param name="id">O indentificador único do cliente </param>
        /// <param name="customer"></param>
        /// <returns>
        /// O cliente com suas propriedades atualizada.
        /// Veja o schema: CustomerGetResponse
        /// </returns>
        /// <remarks>
        /// Exemplos:
        /// 
        ///     ***** O exemplo abaixo permite habilitar um cliente. ******
        ///     
        ///     PATCH /customers
        ///     {
        ///         "op" : "replace",
        ///         "path" : "/isActive",
        ///         "value" : "true"
        ///     }
        /// </remarks>
        /// <response code="200">Retorna ocliente atualizado</response>
        /// <response code="404">Caso o cliente não seja encontrado</response>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id}")]
        public async Task<ActionResult<CustomerGetResponse>> PacthCustomer(Guid id, JsonPatchDocument<Customer> customer)
        {
            var foundedCustomer = await _service.GetCustomer(id);
            if (foundedCustomer == null) return NotFound();

            customer.ApplyTo(foundedCustomer);

            await _service.UpdateCustomer(id, foundedCustomer);
            return Ok(_mapper.Map<CustomerGetResponse>(foundedCustomer));
        }

        
        private readonly CustomerService _service;
        private readonly IMapper _mapper;
        public CustomersController(CustomerService service, IMapper mapper) 
        {
            _service = service;
            _mapper = mapper;
        }

    }
}
