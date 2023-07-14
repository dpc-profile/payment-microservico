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

    public IActionResult EnviarOrdem()
    {
        return View();
    }

    [HttpPost]
    public IActionResult EnviarOrdem(UsuarioInfosModel usuarioInfos)
    {
        return View(usuarioInfos);
    }

    public IActionResult ErroProcura()
    {
        return View();
    }
}