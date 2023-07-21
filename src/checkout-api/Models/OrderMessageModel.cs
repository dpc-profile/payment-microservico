namespace checkout_api.Models;

public class OrderMessageModel
{
    public string? ProdutoIUuid { get; init; }
    public string? ProdutoNome { get; init; }
    public decimal ProdutoPreco { get; init; }
    public string? UsuarioNome { get; init; }
    public string? UsuarioEmail { get; init; }
    public string? UsuarioTelefone { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

}