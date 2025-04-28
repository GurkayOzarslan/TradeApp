using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeAppAPI.Controllers.Commands.Password;
using TradeAppAPI.Controllers.Payloads.Password;
using TradeAppApplication.Commands.Password.ChangePassword;
using TradeAppApplication.Commands.Password.GenerateVerificationCode;
using TradeAppApplication.Commands.Password.VerifyCode;

namespace TradeAppAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommandPayload command)
        {
            var result = await _mediator.Send(new ChangePasswordCommand(command.Email, command.Password));
            return Ok(result);
        }

        [HttpPost("GenerateVerificationCode")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateVerificationCode([FromBody] GenerateVerificationCodeCommandPayload command)
        {
            var result = await _mediator.Send(new GenerateVerificationCodeCommand(command.Email));
            return Ok(result);
        }

        [HttpPost("VerifyCode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodePayload command)
        {
            var result = await _mediator.Send(new VerifyCodeCommand(command.Code, command.Email));
            return Ok(result);
        }
    }
}
