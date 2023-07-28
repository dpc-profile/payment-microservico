using System.ComponentModel.DataAnnotations;

namespace client_app.Models;

public record OrderModel
{
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
}