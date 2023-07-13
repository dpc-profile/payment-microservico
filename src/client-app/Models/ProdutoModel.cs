namespace client_app.Models;
public record ProdutoModel
{
    public string? Uuid { get; init; }
    public string? Produto { get; init; }
    public decimal Preco { get; init; }
}
