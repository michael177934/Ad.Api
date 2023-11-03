using Ad.Core.Models;
using Ad.Core.Response;
using Ad.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
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
        public async Task<ActionResult<OperationResponse<ApplicationUser>>> GetAccount(string id)
        {
            var account = await  _accountRetrieval.GetAccountByUserId(id);
            return Ok(account);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OperationResponse<ApplicationUser>>> UpdateAccount(ApplicationUser user)
        {
            var result = await _accountRetrieval.UpdateProfile(user);
           return Ok(result);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<APIResponse<TransactionResponse>>> TransferFunds(Transaction transaction)
        {
            var result = await _accountRetrieval.TransferFunds(transaction);
            return Ok(result);
            
        }

    }
}
