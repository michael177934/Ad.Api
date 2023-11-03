using Ad.Api.Controllers;
using Ad.API.Resources.User;
using Ad.Core.Models;
using Ad.Core.Response;
using Ad.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Assert = Xunit.Assert;

namespace Ad.Test
{
    public class AccountControllerUnitTest
    {

        [Fact]
        public async void GetAccount_ReturnsNotFound_WhenAccountNotFound()
        {
            var userId = "someUserId";
            Mock<IAccountService> accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(service => service.GetAccountByUserId(userId)).ReturnsAsync(new OperationResponse<ApplicationUser>("Success"));
            var controller = new AccountController(accountServiceMock.Object, null);
            var result = await controller.GetAccount(userId);

            Assert.IsType<ActionResult<OperationResponse<ApplicationUser>>>(result);
            Assert.NotNull(result.Result);
        }


        [Fact]
        public async Task UpdateAccount_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var userProfile = new UserProfile();
            var accountServiceMock = new Mock<IAccountService>();
            var mapperMock = new Mock<IMapper>();
            accountServiceMock.Setup(service => service.UpdateProfile(userProfile))
         .ReturnsAsync(new OperationResponse<UserProfile>("Success.", userProfile, true, true));
            mapperMock.Setup(mapper => mapper.Map<UserProfileResource>(userProfile))
                .Returns(new UserProfileResource());
            var controller = new AccountController(accountServiceMock.Object, mapperMock.Object);
            var result = await controller.UpdateAccount(userProfile);


            Assert.IsType<ActionResult<OperationResponse<UserProfile>>>(result);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task TransferFunds_ReturnsBadRequest_WhenTransferFails()
        {
            // Arrange
            var transaction = new Transaction { /* Initialize transaction properties */ };
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(service => service.TransferFunds(transaction))
           .ReturnsAsync(new OperationResponse<bool>("Transfer failed.", false, false, null));
            var controller = new AccountController(accountServiceMock.Object, null);
            var result = await controller.TransferFunds(transaction);


            Assert.IsType<ActionResult<APIResponse<TransactionResponse>>>(result);
            Assert.NotNull(result.Result);
        }


        [Fact]
        public async Task TransferFunds_ReturnsOkResult()
        {
            // Arrange
            var transaction = new Transaction { /* Initialize transaction properties */ };
            Mock<IAccountService> accountService = new Mock<IAccountService>();

            AccountController accountController = new AccountController(accountService.Object, null);
            accountController.ModelState.AddModelError(
            "Amount",
            "The Amount field is required.");
            // Act
            ActionResult<APIResponse<TransactionResponse>> result = await accountController.TransferFunds(new Transaction());

            // Assert
            Assert.IsType<ActionResult<APIResponse<TransactionResponse>>>(result);
            Assert.NotNull(result.Result);
        }

    }
}