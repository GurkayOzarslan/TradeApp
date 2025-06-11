using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeAppApplication.Queries.Stock.GetFreeStockList;
using TradeAppApplication.Queries.Stock.GetStockDetail;
using TradeAppApplication.Queries.User.GetUserSymbolList;

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
        [HttpGet("GetStockList?Symbol")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStockList([FromQuery] List<string> symbol)
        {
            var result = await _mediator.Send(new GetFreeStockQuery(symbol));
            return Ok(result);
        }

        [HttpGet("GetUserSymbols")]
        public async Task<IActionResult> GetUserSymbols()
        {
            var result = await _mediator.Send(new GetUserSymbolsQuery());
            return Ok(result);
        }

        [HttpGet("GetStockDetailView")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStockDetailView([FromQuery] string symbol)
        {
            var result = await _mediator.Send(new GetStockDetailQuery(symbol));
            return Ok(result);
        }
    }
}
