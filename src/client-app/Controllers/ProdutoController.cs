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
            _logger.LogError(error, "Erro ao procurar o produto.");
            return View("ErroProcura");
        }

        return View(produto);
    }

    [HttpPost]
    public async Task<IActionResult> EnviarOrdem(OrderModel order)
    {
        // Publica mensagem no RabbitMQ
        await _produtoServices.PublicaMensagemAsync(order);

        // Exibir alguma mensagem falando que está sendo processada a compra
        return View();
    }

    public IActionResult ErroProcura()
    {
        return View();
    }
}