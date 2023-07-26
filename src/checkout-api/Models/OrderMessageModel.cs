namespace checkout_api.Models;

public record OrderMessageModel
{
    [Required(ErrorMessage = "O UUID do produto é necessario.")]
    public string? ProdutoIUuid { get; init; }

    [Required(ErrorMessage = "O nome do produto é necessario.")]
    public string? ProdutoNome { get; init; }

    [Required(ErrorMessage = "O preço do produto é necessario.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço do produto deve ser maior que zero.")]
    public decimal ProdutoPreco { get; init; }

    [Required(ErrorMessage = "O nome do usuário é necessario.")]
    public string? UsuarioNome { get; init; }

    [Required(ErrorMessage = "O email do usuário é necessario.")]
    [EmailAddress(ErrorMessage = "O email do usuário não é válido.")]
    public string? UsuarioEmail { get; init; }

    [Required(ErrorMessage = "O telefone do usuário é necessario.")]
    [Phone(ErrorMessage = "O telefone informado é invalido.")]
    public string? UsuarioTelefone { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

}