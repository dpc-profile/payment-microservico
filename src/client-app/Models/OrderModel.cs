namespace client_app.Models;

public record OrderModel
{
    public string? ProdutoUuid { get; init; }
    public string? UsuarioNome { get; init; }
    public string? UsuarioEmail { get; init; }
    public string? UsuarioTelefone { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}