using Ad.API.Resources.Auth;
using Ad.API.Resources.User;
using Ad.Core.Models;
using Ad.Core.Response;
using Ad.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ad.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly HttpContext _httpContext;

        public AuthController(IMapper mapper, ILogger<AuthController> logger, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _logger = logger;
            _authService = authService;
            _httpContext = httpContextAccessor.HttpContext; ;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterResource registerResource)
        {
            var user = _mapper.Map<ApplicationUser>(registerResource);
            var result = await _authService.Register(user, registerResource.Password, GetTenantIdFromRequestHeader());
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginResource loginResource)
        {
            var result = await _authService.Login(loginResource.Username, loginResource.Password, GetTenantIdFromRequestHeader());
            return Ok(result);
        }

        private Guid GetTenantIdFromRequestHeader()
        {
            var tenantId = _httpContext.Request.Headers["Tenant-Id"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(tenantId))
                return Guid.Empty;

            return Guid.Parse(tenantId);
        }
    }
}
