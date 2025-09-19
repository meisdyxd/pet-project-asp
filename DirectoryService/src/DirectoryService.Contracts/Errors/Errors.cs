using DirectoryService.Contracts.Extensions;
using System.Net;

namespace DirectoryService.Contracts.Errors;

public static class Errors
{
    public static class Http
    {
        public static ErrorList InternalServerError()
            => new Error("Internal server error", "internal.exception")
            .ToErrorList((int)HttpStatusCode.InternalServerError);
        
        public static ErrorList BadRequestError(string message) 
            => new Error(message, "http.bad.request")
            .ToErrorList((int)HttpStatusCode.BadRequest);
        
        public static ErrorList Conflict(string message) 
            => new Error(message, "http.conflict")
            .ToErrorList((int)HttpStatusCode.Conflict);

        public static ErrorList NotFound(string message)
            => new Error(message, "http.not.found")
            .ToErrorList((int)HttpStatusCode.NotFound);

        public static ErrorList UnprocessableContent(string message)
            => new Error(message, "http.unprocessable.content")
            .ToErrorList((int)HttpStatusCode.UnprocessableContent);
    }
    public static class InvalidValue
    {
        public static Error Default(string name) 
            => new($"{name} is invalid", "invalid.value", name);
        
        public static Error Empty(string name)
            => new($"{name} is empty", "empty.value", name);

        public static Error IncorrectLength(
            string name, 
            int? minLength = null, 
            int? maxLength = null)
        {
            if (minLength is null && maxLength is null)
                return new($"{name} length have incorrect " +
                                 $"length", "invalid.length", name);
            
            else if (minLength is null && maxLength is not null)
                return new($"{name} length should be " +
                                 $"lower than {maxLength}", "invalid.length", name);
            
            else if (minLength is not null && maxLength is null)
                return new($"{name} length should be higher " +
                                 $"than {minLength}", "invalid.length", name);
            else
                return new($"{name} length should be in range" +
                                 $" {minLength} to {maxLength}", "invalid.length", name);
        }

        /// <summary>
        /// Template error
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Number</param>
        /// <returns>{name} should be greater than {value}</returns>
        public static Error Greater(string name, int value) 
            => new($"{name} should be greater than {value}", "invalid.value", name);

        /// <summary>
        /// Template error
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Number</param>
        /// <returns>{name} should be lesser than {value}</returns>
        public static Error Lesser(string name, int value)
            => new($"{name} should be lesser than {value}", "invalid.value", name);
    }

    public static class DbErrors
    {
        public static ErrorList Default(string name)
            => new Error($"DbError", "db.error")
                .ToErrorList((int)HttpStatusCode.InternalServerError);
        
        public static ErrorList WhenSave(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            => new Error(message, "db.error.save")
                .ToErrorList((int)httpStatusCode);

        public static ErrorList CommitTransaction()
            => new Error("Failed to commit transaction", "transaction.error.commit")
                .ToErrorList((int)HttpStatusCode.InternalServerError);

        public static ErrorList RollbackTransaction()
            => new Error("Failed to rollback transaction", "transaction.error.rollback")
                .ToErrorList((int)HttpStatusCode.InternalServerError);

        public static ErrorList BeginTransaction()
            => new Error("Failed to begin transaction", "transaction.error.begin")
                .ToErrorList((int)HttpStatusCode.InternalServerError);
    }
}