using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Gabriel.DesafioFrontEnd.Api.Controllers.Resources
{
    public class CustomerGetFilter
    {
        /// <summary>
        /// O critério para se fazer a pesquisa nos campos nome e endereço do cliente.
        /// Para se procurar o cliente "Gabriel", apenas passe para este campo gab, ou brie ou el.
        /// O mesmo exemplo acima funciona para o endereço. 
        /// </summary>
        public string? SearchCriteria { get; set; }

        /// <summary>
        /// O número da página que você deseja visualizar começando pela página 0.
        /// Este campo é obrigatório.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        [Range(0, int.MaxValue, ErrorMessage = "Número da página não deve ser um número negativo.")]
        public int PageNumber { get; set; }

        /// <summary>
        /// O tamanho da página que você quer visualizar.
        /// Este campo é obrigatório.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        [Range(0, int.MaxValue, ErrorMessage = "Tamanho da página não deve ser um número negativo.")]
        public int PageSize { get; set; }
    }
}
