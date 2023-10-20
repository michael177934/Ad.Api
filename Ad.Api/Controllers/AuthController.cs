using Ad.API.Resources.Auth;
using Ad.API.Resources.User;
using Ad.Core.Models;
using Ad.Core.Response;
using Ad.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        public AuthController(IMapper mapper, ILogger<AuthController> logger, IAuthService authService, HttpContext httpContext)
        {
            _mapper = mapper;
            _logger = logger;
            _authService = authService;
            _httpContext = httpContext;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterResource registerResource)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(registerResource);
                var result = await _authService.Register(user, registerResource.Password, GetTenantIdFromRequestHeader());

                if (!result.Success)
                    return Ok(APIResponse.Error(result.Message));

                var data = _mapper.Map<UserDetailResource>(result.Data);

                return Ok(APIResponse.Success(result.Message, data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginResource loginResource)
        {
            try
            {
                var result = await _authService.Login(loginResource.Username, loginResource.Password, GetTenantIdFromRequestHeader());

                if (!result.Success)
                    return Ok(APIResponse.Error(result.Message));

                return Ok(APIResponse.Success(result.Message, result.Data, result.ExtraData));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException, ex);
                return BadRequest();
            }
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
