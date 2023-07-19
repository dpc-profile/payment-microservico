namespace client_app.Models;

public record OrderModel
{
    public string? ProdutoId { get; init; }
    public string? Nome { get; init; }
    public string? Email { get; init; }
    public string? Telefone { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}