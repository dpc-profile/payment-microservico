namespace checkout_api.Models;
public record ProdutoModel
{
    public string? Uuid { get; init; }
    public string? Nome { get; init; }
    public decimal Preco { get; init; }
}