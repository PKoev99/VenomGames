namespace VenomGames.Infrastructure.Constants
{
    public static class ErrorMessages
    {
        public const string GameTitleLengthError = "The title must not exceed 100 characters.";
        public const string GameDescriptionLengthError = "The description must not exceed 500 characters.";
        public const string CategoryNameLengthError = "The category name must not exceed 100 characters.";
        public const string ReviewContentLengthError = "The content must not exceed 1000 characters.";
        public const string GamePriceError = "The price must be between 0.01 and 1000.";
    }
}
