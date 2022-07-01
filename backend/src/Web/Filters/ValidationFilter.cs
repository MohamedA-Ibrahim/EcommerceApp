using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Web.Contracts.V1.Responses;
using System.Linq;

namespace Web.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //Check model state on each action execution and return errorMessages
        //To prevent checking state in each action manually
        if (!context.ModelState.IsValid)
        {
            Type type = context.Controller.GetType();
            IEnumerable<System.Reflection.CustomAttributeData> customAttributes = type.CustomAttributes;
            if (customAttributes.Any(x => x.AttributeType == typeof(ApiControllerAttribute)))
            {
                var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();
                var errorResponse = new ErrorResponse();

                foreach (var error in errorsInModelState)
                    foreach (var subError in error.Value)
                    {
                        var errorModel = new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = subError
                        };

                        errorResponse.Errors.Add(errorModel);
                    }
                context.Result = new BadRequestObjectResult(errorResponse);
            }
            else
                await next();
            return;
        }

        await next();
    }
}