namespace client_app.Controllers;

public class ProdutosController : Controller
{
    private readonly IProdutoServices _produtoServices;
    private readonly ILogger _logger;

    public ProdutosController(IProdutoServices produtoServices, ILogger<ProdutosController> logger)
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

    public IActionResult EnviarOrdem()
    {
        return View();
    }

    [HttpPost]
    public IActionResult EnviarOrdem(UsuarioInfosModel usuarioInfos)
    {
        _logger.LogInformation("Objeto de dados", usuarioInfos);
        // Faz o post para checkout-api
        // Exibe uma view simples de compra feita
        return View();
    }

    public IActionResult ErroProcura()
    {
        return View();
    }
}