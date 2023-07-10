namespace produto_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ILogger<ProdutoController> _logger;

    public ProdutoController(ILogger<ProdutoController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public JsonElement GetProdutos()
    {
        return LoadData();
    }

    public JsonElement LoadData()
    {
        string conteudoJson = System.IO.File.ReadAllText("./Repository/products.json");

        // Faz o parsing do conteúdo JSON para um objeto JsonDocument
        JsonDocument jsonDocument = JsonDocument.Parse(conteudoJson);
        var produtos = jsonDocument.RootElement;
        Console.WriteLine();
        return produtos;
    }
}
