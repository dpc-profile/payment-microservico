namespace client_app.Controllers;

public class ProdutoController : Controller
{
    private readonly IProdutoServices _produtoServices;
    private readonly ILogger _logger;

    public ProdutoController(IProdutoServices produtoServices, ILogger<ProdutoController> logger)
    {
        _produtoServices = produtoServices;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<ProdutoModel>? produtos = await _produtoServices.GetProdutosAsync();

        return View(produtos);
    }

    public async Task<IActionResult> ConfirmarOrdem(string uuid)
    {
        ProdutoModel produto;

        try
        {
            produto = await _produtoServices.GetProdutoPorUuidAsync(uuid);
        }
        catch (HttpRequestException error)
        {
            _logger.LogInformation("Eu cai aqui em HttpRequestException", args: error.Message);
            return View("ErroProcura");
        }

        return View(produto);
    }

    [HttpPost]
    public IActionResult EnviarOrdem(UsuarioInfosModel usuarioInfos)
    {
        // Serializa o usuarioInfo para enviar para checkout-api
        byte[] jsonUtf8Bytes =JsonSerializer.SerializeToUtf8Bytes(usuarioInfos);
        
        _logger.LogDebug($"conteudo do json");
        // Enviar dados para a checkout-api

        // Exibir alguma mensagem falando que está sendo processada
        // a compra
        return View(usuarioInfos);
    }

    public IActionResult ErroProcura()
    {
        return View();
    }
}