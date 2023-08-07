namespace checkout_api.Models;
public record ProdutoModel
{
    [Required(ErrorMessage = "O UUID do produto é necessario.")]
    public string? Uuid { get; init; }

    [Required(ErrorMessage = "O nome do produto é necessario.")]
    public string? Nome { get; init; }

    [Required(ErrorMessage = "O preço do produto é necessario.")]
    public decimal Preco { get; init; }
}