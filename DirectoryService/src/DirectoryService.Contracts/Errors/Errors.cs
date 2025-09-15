namespace DirectoryService.Contracts;

public static class Errors
{
    public static class Http
    {
        public static Error InternalServerError()
            => new Error("Internal server error", "internal.exception");
        
        public static Error BadRequestError(string message, string code) 
            => new Error(message, code);
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
        public static Error Default(string name)
            => new($"DbError", "db.error");
        
        public static Error WhenSave(string message)
            => new(message, "db.save.error");
    }
}