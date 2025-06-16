using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeAppApplication.Queries.Stock.GetFreeStockList;
using TradeAppApplication.Queries.Stock.GetStockDetail;
using TradeAppApplication.Queries.Stock.GetSymbolList;
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

        [HttpGet("GetSymbolList")]
        public async Task<IActionResult> GetSymbolList()
        {
            var result = await _mediator.Send(new GetSymbolListQuery());
            return Ok(result);
        }

        [HttpGet("GetStockList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStockList()
        {
            var result = await _mediator.Send(new GetStockList());
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
