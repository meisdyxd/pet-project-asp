namespace DirectoryService.Contracts;

public static class Constants
{
    public static class DepartmentConstants
    {
        public const int MAX_LENGTH_NAME = 150;
        public const int MAX_LENGTH_IDENTIFIER = 150;
        public const int MIN_LENGTH_NAME = 3;
        public const int MIN_LENGTH_IDENTIFIER = 3;
        public const string REGEX_PATH = "^[a-z0-9_-]+(\\.[a-z0-9_-]+)*$";
        public const int MIN_DEPTH = 0;
    }

    public static class PositionConstants
    {
        public const int MAX_LENGTH_NAME = 100;
        public const int MIN_LENGTH_NAME = 3;
        public const int MAX_LENGTH_DESCRIPTION = 1000;
    }

    public static class LocationConstants
    {
        public const int MAX_LENGTH_NAME = 120;
        public const int MIN_LENGTH_NAME = 3;
    }
    
    public static class CommonConstants
    {
        public const string REGEX_IANA = @"^[a-zA-Z_]+/[a-zA-Z_]+(/[a-zA-Z_]+)?$";
        public const bool IS_ACTIVE_DEFAULT = true;
    }
}