namespace produto_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ILogger<ProdutoController> _logger;
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoController(ILogger<ProdutoController> logger, IProdutoRepository produtoRepository)
    {
        _logger = logger;
        _produtoRepository = produtoRepository;
    }

    // GET /api/Produto
    [HttpGet]
    public JsonElement GetAllProdutos()
    {
        return _produtoRepository.BuscarProdutos();
    }

    // GET /api/Produto/uuid
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
            _logger.LogError(message: "Erro ao procurar o produto.", args: error);
            return NotFound("Produto não encontrado");
        }
    }
}