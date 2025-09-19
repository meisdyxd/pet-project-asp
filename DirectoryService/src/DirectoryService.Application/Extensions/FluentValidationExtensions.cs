using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace DirectoryService.Application.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder, string fieldName, string message)
    {
        return ruleBuilder
            .WithName(fieldName)
            .WithErrorCode("invalid.value")
            .WithMessage(message);
    }
    
    public static IRuleBuilderOptionsConditions<T, TProperty>  MustBeValueObject<T, TProperty, TValueObject>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, Result<TValueObject, ErrorList>> createMethod )
    {
        return ruleBuilder.Custom((property, context) =>
        {
            Result<TValueObject, ErrorList> result = createMethod(property);

            if (result.IsSuccess)
                return;

            foreach (var error in result.Error.Errors)
            {
                var validationFailure = new ValidationFailure(error.InvalidField, error.Message)
                {
                    ErrorCode = error.Code
                };
                context.AddFailure(validationFailure);
            }
        });
    }

    public static ErrorList ToErrorList(this ValidationResult result)
    {
        var errors = new ErrorList(
            result.Errors.Select(e 
                => new Error(e.ErrorMessage, e.ErrorCode, e.PropertyName)), 
            HttpStatusCode.BadRequest);
        return errors;
    }
}