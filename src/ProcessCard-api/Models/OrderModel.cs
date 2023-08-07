using System.ComponentModel.DataAnnotations;

namespace ProcessCard_api.Models;

public record OrderModel
{
    public string? PedidoUuid { get; init; }

    [EnumDataType(typeof(StatusEnum), ErrorMessage = "O campo Status deve ter o valor 'Aprovado', 'Pendente', 'Recusado' ou 'Cancelado'.")]
    public string? PedidoStatus { get; set; }

    [Required(ErrorMessage = "O UUID do produto é necessario.")]
    public string? ProdutoUuid { get; init; }

    public decimal? ProdutoPreco {get; init;}

    [Required(ErrorMessage = "O nome do usuário é necessário.")]
    public string? UsuarioNome { get; init; }

    [Required(ErrorMessage = "O email do usuário é necessário.")]
    [EmailAddress(ErrorMessage = "O email não é válido.")]
    public string? UsuarioEmail { get; init; }

    [Required(ErrorMessage = "O telefone do usuário é necessário.")]
    [Phone(ErrorMessage = "O telefone informado é invalido.")]
    public string? UsuarioTelefone { get; init; }

    [Required(ErrorMessage = "A data de criação do pedido é necessaria.")]
    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }
}