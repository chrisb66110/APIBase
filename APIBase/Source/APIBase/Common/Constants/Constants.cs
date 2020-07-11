using System.Diagnostics.CodeAnalysis;

namespace APIBase.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public class BaseConstants
    {
        //Messages
        public const string INVALID_DATA = "Invalid Data.";
        public const string INVALID_ID = "The Id field is required.";
        public const string ERROR_MESSAGE = "An error has occurred.";
        public const string ERROR_MESSAGE_DUPLICATE = "Duplicate error.";
        public const string ERROR_MESSAGE_ENTITY_DONT_EXIST = "The sent entity did not exist.";
        //Const
        public const string PG_DUPLICATE_ERROR = "23505";
        public const string PG_ERROR_DONT_AFFECT_ENTITY = "Database operation expected to affect 1 row(s) but actually affected 0 row(s).";

        //Others const
        public const string ALLOWED_CORS_POLICY = "AllowedCorsPolicy";
    }
}
