using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Helpers.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task<IActionResult?> ValidateAndReturnError<T>(
            this IValidator<T> validator, T dto)
        {
            var result = await validator.ValidateAsync(dto);
            if (result.IsValid) return null;

            return new BadRequestObjectResult(result.Errors.Select(e => new {
                field = e.PropertyName,
                message = e.ErrorMessage
            }));
        }
    }
}
