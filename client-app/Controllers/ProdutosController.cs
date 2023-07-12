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
        _logger.LogInformation("Esse é o valor de uuid:", uuid);
        ProdutoModel produto = await _produtoServices.GetProdutoPorUuidAsync(uuid);
        return View(produto);
    }
}