namespace checkout_api.Models;

public record OrderModel
{
    public string? ProductId {get; init;}
    public string? Nome {get; init;}
    public string? Email {get; init;}
    public string? Telefone {get; init;}
}