using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeAppApplication.Queries.Stock.GetFreeStockList;

namespace TradeAppAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetFreeStockList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFreeStockList([FromQuery] List<string> symbol)
        {
            var result = await _mediator.Send(new GetFreeStockQuery(symbol));
            return Ok(result);
        }
    }
}
