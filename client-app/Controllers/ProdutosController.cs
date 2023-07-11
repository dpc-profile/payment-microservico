namespace client_app.Controllers;

public class ProdutosController : Controller
{
    private readonly IProdutoServices _produtoServices;

    public ProdutosController(IProdutoServices produtoServices)
    {
        _produtoServices = produtoServices;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<ProdutoModel>? produtos = await _produtoServices.GetProdutosAsync();

        return View(produtos);
    }
}