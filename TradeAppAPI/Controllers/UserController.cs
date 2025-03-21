using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TradeAppApplication.Queries.User;

namespace TradeAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new GetLoginUserQuery(request.Email, request.Password));

            if (result == null)
                return Unauthorized("Kullanıcı adı veya şifre hatalı");

            return Ok(result); 
        }
    }
}
