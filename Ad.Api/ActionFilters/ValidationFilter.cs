using System.Linq;
using System.Threading.Tasks;
using Ad.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tarvcent.API.Resources;

namespace Ad.API.ActionFilters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.ErrorCount > 0)
            {
                var errorInModelState = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                var errorResponse = new ErrorResponse();

                foreach (var error in errorInModelState)
                foreach (var subKey in error.Value)
                    errorResponse.Errors.Add(
                        new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = subKey
                        });
                var resonseData = new
                           {ResponseCode = "99", Message = "Input validation error", Data = errorResponse };
                context.Result = new OkObjectResult(resonseData);
            }

            await next();
        }
    }
}