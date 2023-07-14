namespace client_app.Models;

public record UsuarioInfosModel
{
    public string? Nome {get; init;}
    public string? Email {get; init;}
    public string? Telefone {get; init;}
}