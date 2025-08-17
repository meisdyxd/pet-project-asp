namespace DirectoryService.Contracts;

public static class Errors
{
    public static class InvalidValue
    {
        public static Error Default(string name) 
            => new Error($"{name} is invalid", "invalid.value");
        
        public static Error Empty(string name)
            => new Error($"{name} is empty", "empty.value");

        public static Error IncorrectLength(
            string name, 
            int? minLength = null, 
            int? maxLength = null)
        {
            if (minLength is null && maxLength is null)
                return new Error($"{name} length have incorrect " +
                                 $"length", "invalid.length");
            
            if (minLength is null && maxLength is not null)
                return new Error($"{name} length should be " +
                                 $"lower than {maxLength}", "invalid.length");
            
            if (minLength is not null && maxLength is null)
                return new Error($"{name} length should be higher " +
                                 $"than {minLength}", "invalid.length");
            
            return new Error($"{name} length should be in range" +
                             $" {minLength} to {maxLength}", "invalid.length");
        }
    }
}