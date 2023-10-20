using Ad.Core.Models;
using Ad.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Ad.Test
{
    public class AccountControllerUnitTest
    {
        #region Fields
        private readonly AccountController _controller;
        private readonly Mock<IAccountService> _mockAccountService;
        #endregion

        #region ctor
        public AccountControllerUnitTest()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountController(_mockAccountService.Object);
        }
        #endregion

        #region Methods
       
        [Fact]
        public async Task GetAccountByUserIdReturnsOkResult()
        {
            // Arrange
            var expectedAccount = new ApplicationUser();
            var result =  _mockAccountService.Setup(x => x.GetAccountByUserId(It.IsAny<string>())).ReturnsAsync(expectedAccount);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedAccount, okResult.Value);
        }


        
        [Fact]
        public async Task TransferReturnsBadRequest()
        {
            // Arrange
            var transaction = new Transaction
            {
                SenderAccountId = "0123456789"
            };

            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, "testUserId");
            var identity = new ClaimsIdentity(new[] { userIdClaim }, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };

            _mockAccountService.Setup(x => x.GetAccountByUserId(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { AccountNumber = "9876543210" });

            // Act
            var result = await _controller.Transfer(transaction);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        
        #endregion
    }
}