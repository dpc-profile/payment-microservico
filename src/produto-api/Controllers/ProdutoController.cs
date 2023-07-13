namespace produto_api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ILogger<ProdutoController> _logger;
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoController(ILogger<ProdutoController> logger, IProdutoRepository produtoRepository)
    {
        _logger = logger;
        _produtoRepository = produtoRepository;
    }

    // GET /api/v1/Produto
    [HttpGet]
    public JsonElement GetAllProdutos()
    {
        return _produtoRepository.BuscarProdutos();
    }

    // GET /api/v1/Produto/uuid
    [HttpGet]
    [Route("{uuid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<string> GetProdutoPorUuId(string uuid)
    {
        try
        {
            return _produtoRepository.BuscarProdutoPorId(uuid);
        }
        catch (KeyNotFoundException error)
        {
            return NotFound(error.Message);
        }
    }
}