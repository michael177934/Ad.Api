using Ad.API.Resources.User;
using Ad.Core.Models;
using Ad.Core.Response;
using Ad.Core.Services;
using Ad.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

namespace Ad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountRetrieval;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountRetrieval, IMapper mapper)
        {
            _accountRetrieval = accountRetrieval;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetAccount(string id)
        {
            var account = _accountRetrieval.GetAccountByUserId(id);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            return Ok(account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount([FromBody] UserProfile user)
        {
            var result = await _accountRetrieval.UpdateProfile(user);
            if (!result.Success)
                return Ok(APIResponse.Error(result.Message));

            var userResource = _mapper.Map<UserProfileResource>(result.Data);

            return Ok(APIResponse.Success(result.Message, userResource));
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferFunds(string senderAccountId, string recipientAccountId, decimal amount)
        {
            var result = await _accountRetrieval.TransferFunds(senderAccountId, recipientAccountId, amount);

            if (!result.Success)
            {
                return BadRequest("Transfer failed.");
            }

            return Ok("Transfer successful.");
        }

    }
}
