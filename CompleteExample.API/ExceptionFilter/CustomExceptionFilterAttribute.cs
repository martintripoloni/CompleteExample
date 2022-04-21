using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompleteExample.API.ExceptionFilter
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        //Considered it as out of the scope of the challenge but this class should be tested.
        public override void OnException(ExceptionContext context)
        {
            var problemDetailsFactory = context.HttpContext.RequestServices.GetService<ProblemDetailsFactory>();

            if (context.Exception is FluentValidation.ValidationException)
            {
                var validationException = context.Exception as FluentValidation.ValidationException;

                var modelStateDictionary = new ModelStateDictionary();
                foreach (var error in validationException.Errors)
                {
                    modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, modelStateDictionary, (int)HttpStatusCode.BadRequest);
                context.Result = new ObjectResult(problemDetails);
            }            
            else
            {
                var problemDetails = problemDetailsFactory.CreateProblemDetails(context.HttpContext, (int)HttpStatusCode.InternalServerError, detail: "Oops! Something went wrong!");
                context.Result = new ObjectResult(problemDetails);
            }
        }
    }
}
