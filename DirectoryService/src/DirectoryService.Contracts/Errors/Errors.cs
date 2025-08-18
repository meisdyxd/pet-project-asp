namespace DirectoryService.Contracts;

public static class Errors
{
    public static class InvalidValue
    {
        public static Error Default(string name) 
            => new($"{name} is invalid", "invalid.value");
        
        public static Error Empty(string name)
            => new($"{name} is empty", "empty.value");

        public static Error IncorrectLength(
            string name, 
            int? minLength = null, 
            int? maxLength = null)
        {
            if (minLength is null && maxLength is null)
                return new($"{name} length have incorrect " +
                                 $"length", "invalid.length");
            
            else if (minLength is null && maxLength is not null)
                return new($"{name} length should be " +
                                 $"lower than {maxLength}", "invalid.length");
            
            else if (minLength is not null && maxLength is null)
                return new($"{name} length should be higher " +
                                 $"than {minLength}", "invalid.length");
            else
                return new($"{name} length should be in range" +
                                 $" {minLength} to {maxLength}", "invalid.length");
        }

        /// <summary>
        /// Template error
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Number</param>
        /// <returns>{name} should be greater than {value}</returns>
        public static Error Greater(string name, int value) 
            => new($"{name} should be greater than {value}", "invalid.value");

        /// <summary>
        /// Template error
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Number</param>
        /// <returns>{name} should be lesser than {value}</returns>
        public static Error Lesser(string name, int value)
            => new($"{name} should be lesser than {value}", "invalid.value");
    }
}