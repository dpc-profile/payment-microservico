using System.ComponentModel.DataAnnotations;

namespace Order_api.Models;

public record OrderModel
{
    public string? PedidoUuid { get; init; }

    [EnumDataType(typeof(StatusEnum), ErrorMessage = "O campo Status deve ter o valor 'Pendente', 'Recusado' ou 'Aprovado'.")]
    public string? PedidoStatus { get; init; } = "Pendente";

    [Required(ErrorMessage = "O UUID do produto é necessario.")]
    public string? ProdutoUuid { get; init; }

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
}