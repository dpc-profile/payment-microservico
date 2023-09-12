namespace ProcessCard_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessCardController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProcessCardServices _processCardServices;

        public ProcessCardController(ILogger<ProcessCardController> logger, IProcessCardServices processCardServices)
        {
            _logger = logger;
            _processCardServices = processCardServices;
        }

        // POST api/<ProcessCardController>
        [HttpPost]
        public async Task<IActionResult> ProcessarPagamento([FromBody] OrderModel order)
        {
            try
            {
                // Altera status para Aprovado, Recusado ou Cancelado
                _processCardServices.FazerCobranca(order);

                // Devolver a ordem para o order_ex
                await _processCardServices.PostAsync(mensagem: order, uri: "MessageProducer");

                return Ok();
            }
            catch (HttpRequestException error)
            {
                _logger.LogError(message: "Falha em mandar a mensagem pro MessageProducer", args: error);
                return StatusCode(500);
            }
            catch (Exception error)
            {
                _logger.LogError(message: $"Erro não mapeado em {nameof(ProcessarPagamento)}: ", args: error);
                return BadRequest("Erro ao realizar o pagamento.");
            }
        }

    }
}
